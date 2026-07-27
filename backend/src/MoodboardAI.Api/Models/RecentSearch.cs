using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a recent search query made by a user.
/// Duplicate queries update <see cref="CreatedAt"/> instead of creating a new record.
/// </summary>
public class RecentSearch
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Query { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public UserEntity User { get; set; } = null!;
}
