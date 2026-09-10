using MoodboardAI.Api.DTOs.Search;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines search, trending, and category-listing operations backing the
/// Search screen.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Searches pins by free-text query (matched against title, description,
    /// category name, and tag names), optionally filtered by category and/or
    /// tag, with pagination.
    /// </summary>
    /// <param name="query">Free-text search query. When null/empty, no text filter is applied.</param>
    /// <param name="categoryId">Optional category id to filter by.</param>
    /// <param name="tagId">Optional tag id to filter by.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    Task<SearchResponseDto> SearchAsync(string? query, Guid? categoryId, Guid? tagId, int page, int pageSize);

    /// <summary>
    /// Returns the currently trending pins, ranked by like count (most liked first).
    /// </summary>
    /// <param name="count">Maximum number of pins to return.</param>
    Task<List<SearchResultDto>> GetTrendingAsync(int count);

    /// <summary>
    /// Returns all categories available for filtering search results.
    /// </summary>
    Task<List<CategoryDto>> GetCategoriesAsync();
}