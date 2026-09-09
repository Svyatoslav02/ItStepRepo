using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes search, trending, and category-listing
/// endpoints for the Search screen. All endpoints are public (no
/// authentication required).
/// </summary>
[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchController"/> class.
    /// </summary>
    /// <param name="searchService">Search service.</param>
    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    /// <summary>
    /// Searches pins by free-text query (title, description, category, tags),
    /// with optional category/tag filters and pagination.
    /// </summary>
    /// <param name="q">Free-text search query.</param>
    /// <param name="categoryId">Optional category id to filter by.</param>
    /// <param name="tagId">Optional tag id to filter by.</param>
    /// <param name="page">1-based page number (default 1).</param>
    /// <param name="pageSize">Number of items per page (default 10, max 100).</param>
    /// <returns>Paginated list of matching pins. Returns an empty list when nothing matches.</returns>
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] Guid? categoryId,
        [FromQuery] Guid? tagId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _searchService.SearchAsync(q, categoryId, tagId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Returns the currently trending pins, ranked by like count.
    /// </summary>
    /// <param name="count">Maximum number of pins to return (default 10, max 100).</param>
    [HttpGet("trending")]
    public async Task<IActionResult> Trending([FromQuery] int count = 10)
    {
        var result = await _searchService.GetTrendingAsync(count);
        return Ok(result);
    }

    /// <summary>
    /// Returns all categories available for filtering search results.
    /// </summary>
    [HttpGet("categories")]
    public async Task<IActionResult> Categories()
    {
        var result = await _searchService.GetCategoriesAsync();
        return Ok(result);
    }
}