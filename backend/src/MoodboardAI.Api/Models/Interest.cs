using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a selectable interest/category presented to the user
/// during the onboarding flow (e.g. "Minimal", "Photography").
/// </summary>
public class Interest
{
    /// <summary>
    /// Unique identifier of the interest.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Display name of the interest (e.g. "Minimal", "3D Art").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Icon identifier or URL representing the interest in the UI.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the interest record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
