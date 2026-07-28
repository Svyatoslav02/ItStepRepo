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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
