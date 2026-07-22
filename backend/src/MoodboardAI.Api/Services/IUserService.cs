using MoodboardAI.Api.DTOs.Users;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines operations for retrieving user profile data.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the profile of the specified user, including onboarding status and selected interests.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user profile if found; otherwise null.</returns>
    UserProfileDto? GetCurrentUser(string userId);
}