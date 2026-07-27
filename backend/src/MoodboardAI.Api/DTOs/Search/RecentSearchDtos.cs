using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Search;

/// <summary>
/// Represents a single recent search entry returned to the client.
/// </summary>
public class RecentSearchDto
{
    public Guid Id { get; set; }
    public string Query { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Request DTO for saving a recent search query.
/// </summary>
public class AddRecentSearchRequestDto
{
    [Required(ErrorMessage = "Query is required.")]
    [MinLength(1, ErrorMessage = "Query cannot be empty.")]
    [MaxLength(200, ErrorMessage = "Query must be at most 200 characters.")]
    public string Query { get; set; } = string.Empty;
}
