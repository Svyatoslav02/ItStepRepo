using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a notification sent to a user in the application.
/// </summary>
public class Notification
{
    /// <summary>
    /// Unique identifier of the notification.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the user who received the notification.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Identifier of the user who triggered the notification (e.g. the liker, commenter, follower).
    /// </summary>
    public Guid? ActorId { get; set; }

    /// <summary>
    /// Identifier of the related entity (e.g. pin ID, board ID) that the notification references.
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Type of the notification, indicating the nature of the event that triggered it.
    /// </summary>
    public NotificationTypeEnum Type { get; set; }

    /// <summary>
    /// Title of the notification, providing a brief summary of the event.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Message of the notification, providing details about the event.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// URL of the image associated with the notification.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Indicates whether the notification has been read.
    /// </summary>
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Timestamp when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
