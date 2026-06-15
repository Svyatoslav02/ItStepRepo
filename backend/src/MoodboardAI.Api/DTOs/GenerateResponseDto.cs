namespace MoodboardAI.Api.DTOs;

public class GenerateResponseDto
{
    public string Prompt { get; set; } = string.Empty;
    public List<ImageDto> Images { get; set; } = new();
}