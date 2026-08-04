using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.DTOs.Notification;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for managing user notifications.
/// </summary>
[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationController"/> class.
    /// </summary>
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Retrieves a paginated list of notifications for the current user.
    /// Supports optional type filtering. Results are ordered unread-first,
    /// then by creation date descending. Response includes unread count.
    /// </summary>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="type">Optional notification type filter.</param>
    [HttpGet]
    public async Task<ActionResult<NotificationListResponseDto>> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] NotificationTypeEnum? type = null)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid or missing authentication token." });
        }

        var result = await _notificationService.GetNotificationsAsync(userId.Value, page, pageSize, type);
        return Ok(result);
    }

    /// <summary>
    /// Marks a specific notification as read for the current user.
    /// Idempotent: calling again on an already-read notification succeeds.
    /// </summary>
    /// <param name="id">The ID of the notification to mark as read.</param>
    [HttpPost("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid or missing authentication token." });
        }

        var found = await _notificationService.MarkAsReadAsync(userId.Value, id);
        if (!found)
        {
            return NotFound(new ErrorResponse { Message = "Notification not found." });
        }

        return NoContent();
    }

    /// <summary>
    /// Marks all notifications as read for the current user.
    /// Idempotent: succeeds even when there are no unread notifications.
    /// </summary>
    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid or missing authentication token." });
        }

        await _notificationService.MarkAllAsReadAsync(userId.Value);
        return NoContent();
    }

    /// <summary>
    /// Extracts the authenticated user's id from the JWT "sub" claim
    /// (or from <see cref="HttpContext.Items"/> populated by the auth filter).
    /// </summary>
    private Guid? GetCurrentUserId()
    {
        if (HttpContext.Items["UserId"] is Guid contextUserId)
        {
            return contextUserId;
        }

        var subClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(subClaim, out var userId) ? userId : null;
    }
}
