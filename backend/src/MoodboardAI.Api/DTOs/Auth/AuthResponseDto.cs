using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Auth;

/// <summary>
/// Response returned after successful registration or login.
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Signed JWT access token.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Authenticated user's public data.
    /// </summary>
    [Required]
    public UserDto User { get; set; } = new();
}
