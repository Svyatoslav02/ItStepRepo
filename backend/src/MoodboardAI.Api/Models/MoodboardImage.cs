namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents an image item in the generated moodboard response.
/// </summary>
public class MoodboardImage
{
    /// <summary>
    /// Image URL.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Image title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Source URL of the image.
    /// </summary>
    public string SourceUrl { get; set; } = string.Empty;
}