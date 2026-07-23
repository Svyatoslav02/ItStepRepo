using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a "save" interaction between a user and a pin.
/// Each record indicates that a specific user has saved a specific pin to their collection.
/// Prevents duplicate saves through unique constraints.
/// </summary>
public class Save
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Foreign key referencing the pin that was saved.
    /// </summary>
    [Required]
    public Guid PinId { get; set; }
    public Pin? Pin { get; set; }

    /// <summary>
    /// Foreign key referencing the user who saved the pin.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }

    /// <summary>
    /// Timestamp when the save was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}