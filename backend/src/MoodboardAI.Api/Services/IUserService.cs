using MoodboardAI.Api.DTOs.Users;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Interface for user-related services, providing methods 
/// to retrieve information about the current user.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets the unique identifier of the current user.
    /// </summary>
    /// <returns>The unique identifier of the current user.</returns>
    Guid GetCurrentUserId();
    
    /// <summary>
    /// Gets the profile of the current user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>The user profile if found; otherwise null.</returns>
    UserProfileDto? GetCurrentUser(string userId);
}