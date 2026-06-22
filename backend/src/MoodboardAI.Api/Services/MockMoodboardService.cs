using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Mock implementation of moodboard generation service.
/// </summary>
public class MockMoodboardService : IMoodboardService
{
    /// <summary>
    /// Generates a mock moodboard response with predefined image data.
    /// </summary>
    /// <param name="request">Moodboard generation request.</param>
    /// <returns>Generated mock moodboard response.</returns>
    public MoodboardResponse Generate(MoodboardRequest request)
    {
        return new MoodboardResponse
        {
            Prompt = request.Prompt,
            Images = new List<MoodboardImage>
            {
                new()
                {
                    Url = "https://example.com/image1.jpg",
                    Title = "Minimalist interior",
                    SourceUrl = "https://example.com"
                }
            }
        };
    }
}