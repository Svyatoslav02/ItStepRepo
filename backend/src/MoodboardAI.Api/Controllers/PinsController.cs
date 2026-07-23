using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
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
    
    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikePin(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var pin = await _context.Pins.FindAsync(id);
        if (pin == null) return NotFound();

        var exists = await _context.Likes.AnyAsync(l => l.PinId == id && l.UserId == Guid.Parse(userId));
        if (exists) return BadRequest("Already liked.");

        _context.Likes.Add(new Like { PinId = id, UserId = Guid.Parse(userId) });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}/like")]
    public async Task<IActionResult> UnlikePin(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var pin = await _context.Pins.FindAsync(id);
        if(pin == null) return NotFound();
        
        var exists = await _context.Saves.AnyAsync(s => s.PinId == id && s.UserId == Guid.Parse(userId));
        if(exists) return BadRequest("Already liked.");

        _context.Saves.Add(new Save { PinId = id, UserId = Guid.Parse(userId) });
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id}/save")]
    public async Task<IActionResult> UnsavePin(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var save = await _context.Saves.FirstOrDefaultAsync(s => s.PinId == id && s.UserId == userId);
        if (save == null) return NotFound();

        _context.Saves.Remove(save);
        await _context.SaveChangesAsync();

        return Ok();
    }
}