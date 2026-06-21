using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents an application user persisted in the database.
/// Stores authentication data (email, username, password hash) and basic profile information.
/// </summary>
public class UserEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required.")]
    public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(100)]
    public string? DisplayName { get; set; }
    [Range(0, 120, ErrorMessage = "Age must be between 0 and 120.")]
    public int? Age { get; set; }
    [MaxLength(1000)]
    public string? Bio { get; set; }
    [MaxLength(500)]
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}