namespace MoodboardAI.Api.Models;

public class MoodboardResponse
{
    public string Prompt { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
} 
