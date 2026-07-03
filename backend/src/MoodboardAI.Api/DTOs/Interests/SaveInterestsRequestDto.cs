using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Interests;

/// <summary>
/// Request body for saving the current user's selected interests.
/// </summary>
public class SaveInterestsRequestDto
{
    [Required(ErrorMessage = "InterestIds is required.")]
    public List<Guid> InterestIds { get; set; } = new();
}