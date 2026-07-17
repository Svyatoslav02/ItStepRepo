using MoodboardAI.Api.DTOs.Search;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines search operations over pins: filtered/paginated search,
/// trending pins, and the list of available categories.
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// Searches pins by an optional free-text query (matched against title,
    /// description, category name, and tag names), an optional category
    /// filter, and an optional tag filter. Results are paginated.
    /// </summary>
    /// <param name="query">Free-text search query. Null/empty means no text filter.</param>
    /// <param name="categoryId">Optional category id to filter by.</param>
    /// <param name="tag">Optional tag name to filter by.</param>
    /// <param name="page">1-based page number. Values below 1 are treated as 1.</param>
    /// <param name="pageSize">Number of results per page. Clamped to the 1-100 range.</param>
    Task<SearchResponseDto> SearchAsync(string? query, Guid? categoryId, string? tag, int page, int pageSize);

    /// <summary>
    /// Returns the most recently created pins as a simple trending list.
    /// </summary>
    /// <param name="count">Maximum number of pins to return. Clamped to the 1-50 range.</param>
    Task<List<SearchResultDto>> GetTrendingAsync(int count);

    /// <summary>
    /// Returns all pin categories available for filtering.
    /// </summary>
    Task<List<SearchCategoryDto>> GetCategoriesAsync();
}