using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models.Auth;

public class AuthResponseDto
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public UserDto User { get; set; } = new();
}
