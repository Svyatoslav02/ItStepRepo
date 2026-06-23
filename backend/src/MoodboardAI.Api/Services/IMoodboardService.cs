using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines moodboard generation operations.
/// </summary>
public interface IMoodboardService
{
    /// <summary>
    /// Generates a moodboard response based on the provided request.
    /// </summary>
    /// <param name="request">Moodboard generation request.</param>
    /// <returns>Generated moodboard response.</returns>
    MoodboardResponse Generate(MoodboardRequest request);
}