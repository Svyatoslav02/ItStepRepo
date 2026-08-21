using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Extensions;

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
        [FromQuery] PaginationQuery pagination,
        [FromQuery] Guid? categoryId = null,
    	[FromQuery] List<Guid>? tagIds = null)
        {
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

        var pagedResult = await query
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.ImageUrl,
                Category = p.Category.Name,
                Author = p.Author.Username,
                Tags = p.PinTags.Select(pt => pt.Tag.Name),
                InteractionsCount = p.Likes.Count,
                p.CreatedAt
            })
            .ToPagedResultAsync(pagination.Page, pagination.PageSize);
        
        return Ok(pagedResult);
    }
}
