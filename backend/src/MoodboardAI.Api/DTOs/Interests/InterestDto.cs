using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Interests;

/// <summary>
/// Public representation of a selectable onboarding interest.
/// </summary>
public class InterestDto
{
    /// <summary>
    /// Unique identifier of the interest.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Display name of the interest (e.g. "Minimal", "3D Art").
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Icon identifier or URL representing the interest in the UI.
    /// </summary>
    [Required]
    public string Icon { get; set; } = string.Empty;
}
