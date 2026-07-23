using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;


/// <summary>
/// Represents a "like" interaction between a user and a pin.
/// Each record indicates that a specific user has liked a specific pin.
/// Prevents duplicate likes through unique constraints.
/// </summary>
public class Like
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Foreign key referencing the pin that was liked.
    /// </summary>
    [Required]
    public Guid PinId { get; set; }
    public Pin? Pin { get; set; }

    /// <summary>
    /// Foreign key referencing the user who liked the pin.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    /// <summary>
    /// Timestamp when the like was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}