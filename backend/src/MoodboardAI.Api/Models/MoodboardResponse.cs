namespace MoodboardAI.Api.Models;

/// <summary>
/// Response returned when a moodboard is generated. Contains the original prompt and a list of image URLs.
/// </summary>
public class MoodboardResponse
{
    public string Prompt { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
}
