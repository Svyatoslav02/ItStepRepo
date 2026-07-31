using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;

namespace MoodboardAI.Api.Controllers;
[ApiController]
[Route("api/feed")]
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
        List<Guid>? tagIds = null)
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

        var totalCount = await query.CountAsync();

        var pins = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
