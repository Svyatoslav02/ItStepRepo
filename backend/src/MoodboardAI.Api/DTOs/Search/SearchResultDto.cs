namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Represents a single pin returned by search or trending endpoints.
/// </summary>
public class SearchResultDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}