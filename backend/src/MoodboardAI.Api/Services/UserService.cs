using MoodboardAI.Api.DTOs.Users;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Implementation of the IUserService interface, providing methods
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// The IHttpContextAccessor instance used to access the 
    /// current HTTP context and retrieve user information.
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the unique identifier of the current user from the HTTP context.
    /// </summary>
    /// <returns>The unique identifier of the current user.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user ID cannot be parsed.</exception>
    public Guid GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userId, out Guid parsedUserId))
        {
            return parsedUserId;
        }

        throw new InvalidOperationException("Unable to parse user ID.");
    }

    public UserProfileDto? GetCurrentUser(string userId)
    {        
        return new UserProfileDto
        {
            Id = userId,
            FullName = "John Doe",
            Email = "john.doe@example.com",
            AvatarUrl = "https://example.com/avatar.jpg",
            SelectedInterests = new List<DTOs.Interests.InterestDto>(),
            IsOnboardingCompleted = false
        };
    }
}
