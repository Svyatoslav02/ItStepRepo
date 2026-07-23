using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Interests;
using MoodboardAI.Api.DTOs.Users;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Database-backed implementation of <see cref="IUserService"/> that reads
/// the authenticated user's profile, selected interests, and onboarding
/// status from <see cref="ApplicationDbContext"/>.
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public UserProfileDto? GetCurrentUser(string userId)
    {
        if (!Guid.TryParse(userId, out var id))
        {
            return null;
        }

        var user = _dbContext.Users
            .AsNoTracking()
            .FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return null;
        }

        var selectedInterests = _dbContext.UserInterests
            .AsNoTracking()
            .Where(userInterest => userInterest.UserId == id)
            .OrderBy(userInterest => userInterest.Interest.Name)
            .Select(userInterest => new InterestDto
            {
                Id = userInterest.Interest.Id,
                Name = userInterest.Interest.Name,
                Icon = userInterest.Interest.Icon
            })
            .ToList();

        return new UserProfileDto
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            SelectedInterests = selectedInterests,
            IsOnboardingCompleted = user.IsOnboardingCompleted
        };
    }

    /// <summary>
    /// Returns a new GUID as the current user ID for testing purposes.
    /// </summary>
    /// <returns>A new GUID representing the current user ID.</returns>
    public Guid GetCurrentUserId()
    {
        return Guid.NewGuid();
    }
}