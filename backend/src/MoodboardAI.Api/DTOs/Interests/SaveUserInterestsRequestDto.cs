using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Interests;

/// <summary>
/// Request payload for saving the interests selected by the current user
/// during onboarding.
/// </summary>
public class SaveUserInterestsRequestDto
{
    /// <summary>
    /// Identifiers of the interests selected by the user. At least 3 are required.
    /// </summary>
    [Required(ErrorMessage = "InterestIds is required.")]
    [MinLength(3, ErrorMessage = "At least 3 interests are required.")]
    public List<Guid> InterestIds { get; set; } = new();
}
