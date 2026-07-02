using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Auth;

/// <summary>
/// Request payload for authenticating an existing user.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Email address used as the login identifier.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Plain text password supplied by the user, verified against the stored hash.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}
