namespace MoodboardAI.Api.DTOs.Interests;

/// <summary>
/// Response returned after successfully saving the current user's selected interests.
/// </summary>
public class SaveUserInterestsResponseDto
{
    /// <summary>
    /// The interests that are now saved for the current user.
    /// </summary>
    public List<InterestDto> Interests { get; set; } = new();
}
