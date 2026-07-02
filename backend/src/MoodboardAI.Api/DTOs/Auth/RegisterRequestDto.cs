using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Auth;

/// <summary>
/// Request payload for registering a new user account.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// Full display name of the user.
    /// </summary>
    [Required(ErrorMessage = "Full name is required.")]
    [MinLength(2, ErrorMessage = "Full name must be at least 2 characters long.")]
    [MaxLength(100, ErrorMessage = "Full name must be at most 100 characters long.")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address used as the unique login identifier.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email is not valid.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Plain text password supplied by the user. Hashed before persistence.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; } = string.Empty;
}
