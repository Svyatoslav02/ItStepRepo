using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a block relation: the user with <see cref="BlockerId"/> has blocked
/// the user with <see cref="BlockedUserId"/>.
/// Duplicate blocks are prevented via a composite unique key.
/// </summary>
public class BlockedUser
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid BlockerId { get; set; }

    [Required]
    public Guid BlockedUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public UserEntity Blocker { get; set; } = null!;
    public UserEntity Blocked { get; set; } = null!;
}
