using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for the Search screen: filtered/paginated
/// pin search, trending pins, and available categories.
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
    /// Searches pins by an optional free-text query, category, and/or tag.
    /// Matches against pin title, description, category name, and tag names.
    /// </summary>
    /// <param name="q">Free-text search query.</param>
    /// <param name="categoryId">Optional category id filter.</param>
    /// <param name="tag">Optional tag name filter.</param>
    /// <param name="page">1-based page number. Defaults to 1.</param>
    /// <param name="pageSize">Number of results per page. Defaults to 20 (max 100).</param>
    /// <returns>Paginated list of matching pins. An empty result set returns an empty list.</returns>
    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? tag,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var response = await _searchService.SearchAsync(q, categoryId, tag, page, pageSize);

        return Ok(response);
    }

    /// <summary>
    /// Returns the most recently created pins as a simple trending list.
    /// </summary>
    /// <param name="count">Maximum number of pins to return. Defaults to 10 (max 50).</param>
    [HttpGet("trending")]
    public async Task<IActionResult> GetTrending([FromQuery] int count = 10)
    {
        var trending = await _searchService.GetTrendingAsync(count);

        return Ok(trending);
    }

    /// <summary>
    /// Returns all pin categories available for filtering.
    /// </summary>
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _searchService.GetCategoriesAsync();

        return Ok(categories);
    }
}