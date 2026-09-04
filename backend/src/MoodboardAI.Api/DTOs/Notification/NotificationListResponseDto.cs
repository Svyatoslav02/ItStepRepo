using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.DTOs.Notification;

/// <summary>
/// Response DTO for paginated notification lists, including unread count metadata.
/// </summary>
public class NotificationListResponseDto : PagedResult<NotificationDto>
{
    public int UnreadCount { get; set; }
}
