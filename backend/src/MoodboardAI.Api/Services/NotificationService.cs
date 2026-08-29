using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Notification;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// EF Core-backed implementation of <see cref="INotificationService"/>.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationService"/> class.
    /// </summary>
    public NotificationService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task<NotificationListResponseDto> GetNotificationsAsync(
        Guid userId,
        int page = 1,
        int pageSize = 10,
        NotificationTypeEnum? type = null)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        if (pageSize > 100)
        {
            pageSize = 100;
        }

        var query = _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId);

        if (type.HasValue)
        {
            query = query.Where(n => n.Type == type.Value);
        }

        var totalCount = await query.CountAsync();

        var unreadCount = await _dbContext.Notifications
            .AsNoTracking()
            .CountAsync(n => n.UserId == userId && !n.IsRead);

        var entities = await query
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new NotificationListResponseDto
        {
            Items = entities.Select(ToDto).ToList(),
            TotalCount = totalCount,
            UnreadCount = unreadCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc />
    public async Task<bool> MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        var notification = await _dbContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notification is null)
        {
            return false;
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            await _dbContext.SaveChangesAsync();
        }

        return true;
    }

    /// <inheritdoc />
    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var unread = await _dbContext.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        if (unread.Count == 0)
        {
            return;
        }

        foreach (var notification in unread)
        {
            notification.IsRead = true;
        }

        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<NotificationDto?> CreateAsync(
        Guid recipientUserId,
        NotificationTypeEnum type,
        Guid? actorId,
        Guid? relatedEntityId,
        string title,
        string message,
        string? imageUrl = null)
    {
        // Self-actions should not produce a notification for the actor.
        if (actorId.HasValue && actorId.Value == recipientUserId)
        {
            return null;
        }

        var notification = new Notification
        {
            UserId = recipientUserId,
            ActorId = actorId,
            RelatedEntityId = relatedEntityId,
            Type = type,
            Title = title,
            Message = message,
            ImageUrl = imageUrl,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();

        return ToDto(notification);
    }

    /// <inheritdoc />
    public Task<NotificationDto?> NotifyLikeAsync(
        Guid recipientUserId,
        Guid actorId,
        Guid pinId,
        string actorDisplayName)
    {
        return CreateAsync(
            recipientUserId,
            NotificationTypeEnum.Like,
            actorId,
            pinId,
            "New like",
            $"{actorDisplayName} liked your pin.");
    }

    /// <inheritdoc />
    public Task<NotificationDto?> NotifyCommentAsync(
        Guid recipientUserId,
        Guid actorId,
        Guid pinId,
        string actorDisplayName)
    {
        return CreateAsync(
            recipientUserId,
            NotificationTypeEnum.Comment,
            actorId,
            pinId,
            "New comment",
            $"{actorDisplayName} commented on your pin.");
    }

    /// <inheritdoc />
    public Task<NotificationDto?> NotifyNewFollowerAsync(
        Guid recipientUserId,
        Guid actorId,
        string actorDisplayName)
    {
        return CreateAsync(
            recipientUserId,
            NotificationTypeEnum.NewFollower,
            actorId,
            relatedEntityId: null,
            title: "New follower",
            message: $"{actorDisplayName} started following you.");
    }

    private static NotificationDto ToDto(Notification n) => new()
    {
        Id = n.Id,
        ActorId = n.ActorId,
        RelatedEntityId = n.RelatedEntityId,
        Type = n.Type,
        Title = n.Title,
        Message = n.Message,
        ImageUrl = n.ImageUrl,
        IsRead = n.IsRead,
        CreatedAt = n.CreatedAt
    };
}
