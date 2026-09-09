using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Represents a category available for filtering, as returned by
/// <c>GET /api/search/categories</c>.
/// </summary>
public class CategoryDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Icon { get; set; } = string.Empty;
}