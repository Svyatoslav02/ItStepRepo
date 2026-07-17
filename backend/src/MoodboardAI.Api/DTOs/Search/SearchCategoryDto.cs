namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Public representation of a pin category, as returned by GET /api/search/categories.
/// </summary>
public class SearchCategoryDto
{
    /// <summary>
    /// Unique identifier of the category.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name of the category.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Icon identifier representing the category in the UI.
    /// </summary>
    public string Icon { get; set; } = string.Empty;
}