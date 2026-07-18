using MoodboardAI.Api.DTOs.Interests;
using MoodboardAI.Api.DTOs.Users;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Mock implementation of <see cref="IUserService"/> returning predefined profile data.
/// </summary>
public class MockUserService : IUserService
{
    /// <summary>
    /// Returns a fixed mock profile for any given userId.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A mock user profile.</returns>
    public UserProfileDto? GetCurrentUser(string userId)
    {
        return new UserProfileDto
        {
            Id = userId,
            FullName = "Jane Doe",
            Email = "jane.doe@example.com",
            AvatarUrl = "https://example.com/avatars/jane.jpg",
            SelectedInterests = new List<InterestDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Minimalism", Icon = "minimal.svg" },
                new() { Id = Guid.NewGuid(), Name = "Japanese style", Icon = "japan.svg" }
            },
            IsOnboardingCompleted = true
        };
    }
}