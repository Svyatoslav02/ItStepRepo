using MoodboardAI.Api.DTOs.Interests;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines operations for retrieving available interests and managing a user's selected interests.
/// </summary>
public interface IInterestService
{
    /// <summary>
    /// Returns all available interests.
    /// </summary>
    /// <returns>List of available interests.</returns>
    List<InterestDto> GetAllInterests();

    /// <summary>
    /// Validates and saves the selected interests for the given user.
    /// </summary>
    /// <param name="userId">Identifier of the current authenticated user.</param>
    /// <param name="interestIds">Identifiers of the selected interests.</param>
    /// <returns>
    /// A tuple where <c>Success</c> indicates whether the operation succeeded,
    /// and <c>ErrorMessage</c> contains the validation error when it did not.
    /// </returns>
    (bool Success, string? ErrorMessage) SaveUserInterests(string userId, List<Guid> interestIds);
}