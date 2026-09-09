namespace MoodboardAI.Api.DTOs.Users;

/// <summary>
/// Internal result returned by the user service layer to the controller
/// after attempting to update the current user's profile.
/// </summary>
public class UpdateUserProfileResultDto
{
    /// <summary>
    /// Whether the update operation succeeded.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Error message describing why the operation failed. Only set when <see cref="Succeeded"/> is false.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// The updated profile. Only set when <see cref="Succeeded"/> is true.
    /// </summary>
    public UserProfileDto? Profile { get; set; }
}