using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Pins;
using MoodboardAI.Api.Extensions;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Controllers;

[ApiController]
[Route("api/pins")]
public class PinsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PinsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPinId(Guid id)
    {
        var pin = await _context.Pins
            .Include(p => p.Category)
            .Include(p => p.PinTags).ThenInclude(p => p.Tag)
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pin == null) return NotFound();

        return Ok(new
        {
            pin.Id,
            pin.Title,
            pin.Description,
            pin.ImageUrl,
            pin.SourceUrl,
            Author = pin.Author.Username,
            Category = pin.Category.Name,
            Tags = pin.PinTags.Select(pt => pt.Tag.Name),
            LikeCount = pin.Likes.Count,
            pin.CreatedAt
        });
    }

    /// <summary>
    /// Creates a new pin owned by the authenticated user.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePin([FromBody] CreatePinRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErrorResponse());
        }

        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (!categoryExists)
        {
            return BadRequest(new ErrorResponse { Message = "Category not found." });
        }

        var pin = new Pin
        {
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            SourceUrl = request.SourceUrl,
            AuthorId = userId.Value,
            CategoryId = request.CategoryId
        };

        // Attach tags, reusing existing ones (matched case-insensitively) and
        // creating new ones on the fly.
        var normalizedTagNames = request.Tags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (normalizedTagNames.Count > 0)
        {
            var existingTags = await _context.Tags
                .Where(t => normalizedTagNames.Contains(t.Name))
                .ToListAsync();

            foreach (var tagName in normalizedTagNames)
            {
                var tag = existingTags.FirstOrDefault(t =>
                    string.Equals(t.Name, tagName, StringComparison.OrdinalIgnoreCase));

                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                }

                pin.PinTags.Add(new PinTag { Tag = tag });
            }
        }

        _context.Pins.Add(pin);
        await _context.SaveChangesAsync();

        // Reload with navigation properties for a consistent response shape.
        await _context.Entry(pin).Reference(p => p.Author).LoadAsync();
        await _context.Entry(pin).Reference(p => p.Category).LoadAsync();
        await _context.Entry(pin).Collection(p => p.PinTags).Query().Include(pt => pt.Tag).LoadAsync();

        var response = new PinResponseDto
        {
            Id = pin.Id,
            Title = pin.Title,
            Description = pin.Description,
            ImageUrl = pin.ImageUrl,
            SourceUrl = pin.SourceUrl,
            Author = pin.Author.Username,
            Category = pin.Category.Name,
            Tags = pin.PinTags.Select(pt => pt.Tag.Name),
            LikeCount = 0,
            CreatedAt = pin.CreatedAt
        };

        return CreatedAtAction(nameof(GetPinId), new { id = pin.Id }, response);
    }

    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikePin(Guid id)
    {

        var userIdString = User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }
        var userId = Guid.Parse(userIdString);

        var pin = await _context.Pins.FindAsync(id);
        if (pin == null) return NotFound();

        var exists = await _context.Likes.AnyAsync(l => l.PinId == id && l.UserId == userId);
        if (exists) return BadRequest("Already liked.");

        _context.Likes.Add(new Like { PinId = id, UserId = userId });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}/like")]
    public async Task<IActionResult> UnlikePin(Guid id)
    {

        var userIdString = User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }
        var userId = Guid.Parse(userIdString);

        var like = await _context.Likes.FirstOrDefaultAsync(l => l.PinId == id && l.UserId == userId);
        if (like == null) return NotFound();

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpPost("{id}/save")]
    public async Task<IActionResult> SavePin(Guid id)
    {

        var userIdString = User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }
        var userId = Guid.Parse(userIdString);
        var pin = await _context.Pins.FindAsync(id);
        if (pin == null) return NotFound();

        var exists = await _context.Saves.AnyAsync(s => s.PinId == id && s.UserId == userId);
        if (exists) return BadRequest("Already saved.");


        _context.Saves.Add(new Save { PinId = id, UserId = userId });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}/save")]
    public async Task<IActionResult> UnsavePin(Guid id)
    {

        var userIdString = User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized();
        }
        var userId = Guid.Parse(userIdString);

        var save = await _context.Saves.FirstOrDefaultAsync(s => s.PinId == id && s.UserId == userId);
        if (save == null) return NotFound();

        _context.Saves.Remove(save);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // ──────────────────────────────────────────────
    // Comments
    // ──────────────────────────────────────────────

    /// <summary>
    /// Returns the comments posted on a pin, oldest first.
    /// </summary>
    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetComments(Guid id)
    {
        var pinExists = await _context.Pins.AnyAsync(p => p.Id == id);
        if (!pinExists) return NotFound(new ErrorResponse { Message = "Pin not found." });

        var comments = await _context.Comments
            .Where(c => c.PinId == id)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Text = c.Text,
                AuthorId = c.AuthorId,
                AuthorUsername = c.Author.Username,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(comments);
    }

    /// <summary>
    /// Posts a new comment on a pin as the authenticated user.
    /// </summary>
    [Authorize]
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(Guid id, [FromBody] CreateCommentRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErrorResponse());
        }

        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var pinExists = await _context.Pins.AnyAsync(p => p.Id == id);
        if (!pinExists) return NotFound(new ErrorResponse { Message = "Pin not found." });

        var comment = new Comment
        {
            PinId = id,
            AuthorId = userId.Value,
            Text = request.Text.Trim()
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        await _context.Entry(comment).Reference(c => c.Author).LoadAsync();

        var response = new CommentDto
        {
            Id = comment.Id,
            Text = comment.Text,
            AuthorId = comment.AuthorId,
            AuthorUsername = comment.Author.Username,
            CreatedAt = comment.CreatedAt
        };

        return CreatedAtAction(nameof(GetComments), new { id }, response);
    }

    // ──────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────

    private Guid? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        return claim != null && Guid.TryParse(claim.Value, out var id) ? id : null;
    }
}