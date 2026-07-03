using MoodboardAI.Api.DTOs.Interests;
using System.Linq;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Mock implementation of interest retrieval and selection.
/// Uses an in-memory list instead of a real database.
/// </summary>
public class MockInterestService : IInterestService
{
    private const int MinimumRequiredInterests = 3;

    private static readonly List<InterestDto> AvailableInterests = new()
    {
        new InterestDto { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Minimalism", Icon = "minimalism.svg" },
        new InterestDto { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Japanese style", Icon = "japanese.svg" },
        new InterestDto { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Boho", Icon = "boho.svg" },
        new InterestDto { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Scandinavian", Icon = "scandinavian.svg" },
        new InterestDto { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Industrial", Icon = "industrial.svg" }
    };

    /// <summary>
    /// Returns all available interests.
    /// </summary>
    /// <returns>List of available interests.</returns>
    public List<InterestDto> GetAllInterests()
    {
        return AvailableInterests;
    }

    /// <summary>
    /// Validates and saves the selected interests for the given user.
    /// </summary>
    /// <param name="userId">Identifier of the current authenticated user.</param>
    /// <param name="interestIds">Identifiers of the selected interests.</param>
    /// <returns>
    /// A tuple where <c>Success</c> indicates whether the operation succeeded,
    /// and <c>ErrorMessage</c> contains the validation error when it did not.
    /// </returns>
    public (bool Success, string? ErrorMessage) SaveUserInterests(string userId, List<Guid> interestIds)
    {
        if (interestIds.Count < MinimumRequiredInterests)
        {
            return (false, $"At least {MinimumRequiredInterests} interests are required.");
        }

        var validIds = AvailableInterests.Select(i => i.Id).ToHashSet();
        var hasInvalidId = interestIds.Any(id => !validIds.Contains(id));

        if (hasInvalidId)
        {
            return (false, "One or more interest ids are invalid.");
        }

        // Mock: no real persistence yet.
        return (true, null);
    }
}