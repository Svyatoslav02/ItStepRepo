using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models.Auth;

public class UserDto
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
}
