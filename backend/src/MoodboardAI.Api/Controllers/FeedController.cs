using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Extensions;
using MoodboardAI.Api.DTOs;

namespace MoodboardAI.Api.Controllers;
[ApiController]
[Route("api/v1/feed")]
public class FeedController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FeedController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetFeed(
        int page = 1,
        int pageSize = 10,
        Guid? categoryId = null,
        List<Guid>? tagIds = null,
        string? sort = "newest")
    {
        if (page <= 0 || pageSize <= 0)
            return BadRequest("Invalid pagination values.");
        
        pageSize = Math.Min(pageSize, 100);
        var query = _context.Pins
            .Include(p => p.Category)
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Include(p => p.PinTags)
            .ThenInclude(pt => pt.Tag)
            .OrderByDescending(p => p.CreatedAt)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (tagIds != null && tagIds.Any())
            query = query.Where(p => p.PinTags.Any(pt => tagIds.Contains(pt.TagId)));

        var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var interests = await _context.UserInterests
                .Where(ui => ui.UserId == Guid.Parse(userId))
                .Select(ui => ui.InterestId)
                .ToListAsync();

            if (interests.Any())
                query = query.Where(p => interests.Contains(p.CategoryId));
        }

        query = sort?.ToLower() switch
        {
            "popular" => query.OrderByDescending(p => p.Likes.Count),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };
        var totalCount = await query.CountAsync();

        var pins = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new FeedItemDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                Category = p.Category.Name,
                Author = p.Author.Username,
                Tags = p.PinTags.Select(pt => pt.Tag.Name).ToList(),
                LikeCount = p.Likes.Count,
                IsLiked = userId != null && p.Likes.Any(l => l.UserId == Guid.Parse(userId)),
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = pins
        });
    }
}
