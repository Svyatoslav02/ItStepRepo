using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Request shape for moodboard generation. Contains the textual prompt used to create a moodboard.
/// </summary>
public class MoodboardRequest
{
    [Required(ErrorMessage = "Prompt is required.")]
    [MinLength(3, ErrorMessage = "Prompt must be at least 3 characters long.")]
    public string Prompt { get; set; } = string.Empty;
} 
