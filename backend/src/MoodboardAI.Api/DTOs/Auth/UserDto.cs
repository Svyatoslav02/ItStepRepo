using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Auth;

/// <summary>
/// Public-facing user data returned as part of authentication responses.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    [Required]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Full display name of the user.
    /// </summary>
    [Required]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the user.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;
}
