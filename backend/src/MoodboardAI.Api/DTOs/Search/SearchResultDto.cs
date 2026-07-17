namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Public representation of a single pin returned by search and trending endpoints.
/// </summary>
public class SearchResultDto
{
    /// <summary>
    /// Unique identifier of the pin.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the pin.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// URL of the pin's image.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Name of the category the pin belongs to.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Names of the tags attached to the pin.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// UTC timestamp when the pin was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}