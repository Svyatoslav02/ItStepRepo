using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Interests;
using MoodboardAI.Api.DTOs.Privacy;
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

        var privacySettings = _dbContext.UserPrivacySettings
            .AsNoTracking()
            .FirstOrDefault(p => p.UserId == id);

        var privacy = privacySettings is null
            ? new PrivacySettingsDto { PrivateAccount = false, SearchVisibility = true, ContentVisibility = true }
            : new PrivacySettingsDto
            {
                PrivateAccount = privacySettings.PrivateAccount,
                SearchVisibility = privacySettings.SearchVisibility,
                ContentVisibility = privacySettings.ContentVisibility
            };

        return new UserProfileDto
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            SelectedInterests = selectedInterests,
            IsOnboardingCompleted = user.IsOnboardingCompleted,
            Privacy = privacy
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

    /// <inheritdoc />
    public async Task<UpdateUserProfileResultDto> UpdateCurrentUserAsync(string userId, UpdateUserProfileDto request)
    {
        if (!Guid.TryParse(userId, out var id))
        {
            return new UpdateUserProfileResultDto { Succeeded = false, ErrorMessage = "Invalid user id." };
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            return new UpdateUserProfileResultDto { Succeeded = false, ErrorMessage = "User not found." };
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var emailTaken = await _dbContext.Users
                .AnyAsync(u => u.Id != id && u.Email.ToLower() == normalizedEmail);

            if (emailTaken)
            {
                return new UpdateUserProfileResultDto
                {
                    Succeeded = false,
                    ErrorMessage = "A user with this email already exists."
                };
            }

            user.Email = request.Email.Trim();
        }

        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            user.FullName = request.FullName.Trim();
        }

        if (request.AvatarUrl is not null)
        {
            var trimmedAvatarUrl = request.AvatarUrl.Trim();
            user.AvatarUrl = trimmedAvatarUrl.Length == 0 ? null : trimmedAvatarUrl;
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return new UpdateUserProfileResultDto
        {
            Succeeded = true,
            Profile = GetCurrentUser(userId)
        };
    }
}