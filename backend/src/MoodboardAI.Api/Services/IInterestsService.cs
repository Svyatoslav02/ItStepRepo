using MoodboardAI.Api.DTOs.Interests;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines operations for listing available onboarding interests and
/// saving the interests selected by a user.
/// </summary>
public interface IInterestsService
{
    /// <summary>
    /// Returns all interests available for selection.
    /// </summary>
    Task<List<InterestDto>> GetAllAsync();

    /// <summary>
    /// Saves the set of interests selected by the given user, replacing any
    /// previously saved selection. Fails if any of the supplied interest ids
    /// does not correspond to an existing interest.
    /// </summary>
    /// <param name="userId">Identifier of the user the interests are saved for.</param>
    /// <param name="interestIds">Identifiers of the interests selected by the user.</param>
    Task<SaveUserInterestsResultDto> SaveUserInterestsAsync(Guid userId, IEnumerable<Guid> interestIds);
}
