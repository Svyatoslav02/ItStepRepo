using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.DTOs.Notification;

/// <summary>
/// Response DTO for paginated notification lists, including unread count metadata.
/// </summary>
public class NotificationListResponseDto
{
    /// <summary>
    /// The notifications for the current page.
    /// </summary>
    public List<NotificationDto> Items { get; set; } = new();

    /// <summary>
    /// Total number of notifications for the current user.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of unread notifications for the current user.
    /// </summary>
    public int UnreadCount { get; set; }

    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; }
}
