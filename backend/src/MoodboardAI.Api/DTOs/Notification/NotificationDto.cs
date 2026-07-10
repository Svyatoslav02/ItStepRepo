using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.DTOs.Notification;

/// <summary>
/// Represents a data transfer object (DTO) for a notification, used to transfer 
/// notification data between the API and clients.
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the notification.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the type of the notification.
    /// </summary>
    public NotificationType Type { get; set; }
    /// <summary>
    /// Gets or sets the title of the notification.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the message of the notification.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the URL of the image associated with the notification.
    /// </summary>
    public string? ImageUrl { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the notification has been read.
    /// </summary>
    public bool IsRead { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

