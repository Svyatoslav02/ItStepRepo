namespace MoodboardAI.Api.Models;

/// <summary>
/// Response returned when a moodboard is generated.
/// Contains the original prompt and a list of moodboard images.
/// </summary>
public class MoodboardResponse
{
    /// <summary>
    /// Original user prompt.
    /// </summary>
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// List of generated moodboard images.
    /// </summary>
    public List<MoodboardImage> Images { get; set; } = new();
}