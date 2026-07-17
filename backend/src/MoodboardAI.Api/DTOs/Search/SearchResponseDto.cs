namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Paginated response returned by GET /api/search.
/// </summary>
public class SearchResponseDto
{
    /// <summary>
    /// Pins matching the search criteria for the current page.
    /// </summary>
    public List<SearchResultDto> Results { get; set; } = new();

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pins matching the search criteria, across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages available for the current criteria.
    /// </summary>
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}