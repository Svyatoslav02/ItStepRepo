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
