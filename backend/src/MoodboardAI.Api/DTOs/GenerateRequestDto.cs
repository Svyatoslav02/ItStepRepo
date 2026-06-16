using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs;

public class GenerateRequestDto
{
    [Required]
    [MinLength(3)]
    public string Prompt { get; set; } = string.Empty;
}