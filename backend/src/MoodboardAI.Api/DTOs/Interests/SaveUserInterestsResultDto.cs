namespace MoodboardAI.Api.DTOs.Interests;

/// <summary>
/// Internal result returned by the interests service layer to the controller.
/// </summary>
public class SaveUserInterestsResultDto
{
    /// <summary>
    /// Whether the save operation succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Error message describing why the operation failed. Only set when <see cref="Succeeded"/> is false.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// The interests saved for the user. Only set when <see cref="Succeeded"/> is true.
    /// </summary>
    public List<InterestDto>? Interests { get; set; }
}
