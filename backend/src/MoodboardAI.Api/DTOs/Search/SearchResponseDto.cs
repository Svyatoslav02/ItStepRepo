namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Paginated response returned by <c>GET /api/search</c>.
/// </summary>
public class SearchResponseDto
{
    public List<SearchResultDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}