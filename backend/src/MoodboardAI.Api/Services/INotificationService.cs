using MoodboardAI.Api.DTOs.Notification;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Defines operations for creating, listing, and marking user notifications as read.
/// Social action endpoints (like, comment, follow) should call the typed create helpers
/// so notifications are produced automatically when those actions occur.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Returns a paginated list of notifications for the given user, optionally
    /// filtered by type. Results are ordered unread-first, then by creation date descending.
    /// </summary>
    Task<NotificationListResponseDto> GetNotificationsAsync(
        Guid userId,
        int page = 1,
        int pageSize = 10,
        NotificationTypeEnum? type = null);

    /// <summary>
    /// Marks a single notification as read. Idempotent: already-read notifications succeed.
    /// </summary>
    /// <returns><c>true</c> if the notification was found and belongs to the user; otherwise <c>false</c>.</returns>
    Task<bool> MarkAsReadAsync(Guid userId, Guid notificationId);

    /// <summary>
    /// Marks all unread notifications for the user as read. Idempotent when none are unread.
    /// </summary>
    Task MarkAllAsReadAsync(Guid userId);

    /// <summary>
    /// Creates a notification for the recipient. Skips creation when the actor is the recipient
    /// (self-actions do not generate notifications).
    /// </summary>
    Task<NotificationDto?> CreateAsync(
        Guid recipientUserId,
        NotificationTypeEnum type,
        Guid? actorId,
        Guid? relatedEntityId,
        string title,
        string message,
        string? imageUrl = null);

    /// <summary>
    /// Creates a Like notification for the pin owner when another user likes their pin.
    /// Intended to be called from pin like endpoints (Task #4).
    /// </summary>
    Task<NotificationDto?> NotifyLikeAsync(Guid recipientUserId, Guid actorId, Guid pinId, string actorDisplayName);

    /// <summary>
    /// Creates a Comment notification for the pin owner when another user comments.
    /// Intended to be called from comment endpoints (Task #4).
    /// </summary>
    Task<NotificationDto?> NotifyCommentAsync(Guid recipientUserId, Guid actorId, Guid pinId, string actorDisplayName);

    /// <summary>
    /// Creates a NewFollower notification when another user follows the recipient.
    /// Intended to be called from follow endpoints (Task #4).
    /// </summary>
    Task<NotificationDto?> NotifyNewFollowerAsync(Guid recipientUserId, Guid actorId, string actorDisplayName);
}
