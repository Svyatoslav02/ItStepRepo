using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Interests;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Implements listing of available onboarding interests and saving of the
/// interests selected by a user, backed by <see cref="ApplicationDbContext"/>.
/// </summary>
public class InterestsService : IInterestsService
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterestsService"/> class.
    /// </summary>
    public InterestsService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<List<InterestDto>> GetAllAsync()
    {
        return await _dbContext.Interests
            .OrderBy(interest => interest.Name)
            .Select(interest => new InterestDto
            {
                Id = interest.Id,
                Name = interest.Name,
                Icon = interest.Icon
            })
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<SaveUserInterestsResultDto> SaveUserInterestsAsync(Guid userId, IEnumerable<Guid> interestIds)
    {
        var requestedIds = interestIds.Distinct().ToList();

        var matchingInterests = await _dbContext.Interests
            .Where(interest => requestedIds.Contains(interest.Id))
            .ToListAsync();

        if (matchingInterests.Count != requestedIds.Count)
        {
            return new SaveUserInterestsResultDto
            {
                Succeeded = false,
                ErrorMessage = "One or more interest ids are invalid."
            };
        }

        // Replace any previously saved selection with the new one. The
        // composite unique key on (UserId, InterestId) prevents duplicate
        // rows even if this logic is ever bypassed.
        var existingSelections = await _dbContext.UserInterests
            .Where(userInterest => userInterest.UserId == userId)
            .ToListAsync();

        _dbContext.UserInterests.RemoveRange(existingSelections);

        var newSelections = matchingInterests
            .Select(interest => new UserInterest
            {
                UserId = userId,
                InterestId = interest.Id
            })
            .ToList();

        await _dbContext.UserInterests.AddRangeAsync(newSelections);

        // AC: saving valid interests (>=3) marks onboarding as completed.
        // AC: fewer than 3 does not complete onboarding — DTO-level [MinLength(3)]
        // already blocks this at the controller, this is the service-layer guard.
        if (newSelections.Count >= 3)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is not null && !user.IsOnboardingCompleted)
            {
                user.IsOnboardingCompleted = true;
                user.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _dbContext.SaveChangesAsync();

        return new SaveUserInterestsResultDto
        {
            Succeeded = true,
            Interests = matchingInterests
                .OrderBy(interest => interest.Name)
                .Select(interest => new InterestDto
                {
                    Id = interest.Id,
                    Name = interest.Name,
                    Icon = interest.Icon
                })
                .ToList()
        };
    }
}
