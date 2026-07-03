using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents an application user persisted in the database.
/// Stores authentication data (email, username, password hash) and basic profile information.
/// </summary>
[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [MaxLength(100)]
    public string? DisplayName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    [MaxLength(1000)]
    public string? Bio { get; set; }
    [MaxLength(500)]
    public string? AvatarUrl { get; set; }
    public bool IsOnboardingCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}