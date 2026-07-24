using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Notification;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Filters;
using System.Security.Claims;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for managing user notifications.
/// </summary>
[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NotificationController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the current user's ID from the HttpContext.Items.
    /// </summary>
    /// <returns>The user's ID.</returns>
    private Guid GetCurrentUserId()
    {
        if (HttpContext.Items["UserId"] is Guid userId)
            return userId;

        throw new UnauthorizedAccessException("User ID not found in HttpContext.");
    }

    /// <summary>
    /// Retrieves a paginated list of notifications for the current user, 
    /// ordered by creation date descending.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of notifications to retrieve per page.</param>
    /// <returns>A list of notifications for the current user.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(int page = 1, int pageSize = 10)
    {
        var userId = GetCurrentUserId();

        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Type = n.Type,
                Title = n.Title,
                Message = n.Message,
                ImageUrl = n.ImageUrl,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return Ok(notifications);
    }

    /// <summary>
    /// Marks a specific notification as read for the current user.
    /// </summary>
    /// <param name="id">The ID of the notification to mark as read.</param>
    /// <returns>No content if successful, not found
    /// if the notification does not exist.</returns>
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var userId = GetCurrentUserId();
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (notification == null) return NotFound();

        notification.IsRead = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Marks all notifications as read for the current user.
    /// </summary>
    /// <returns>No content if successful.</returns>
    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = GetCurrentUserId();

        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync();

        foreach (var n in notifications) n.IsRead = true;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Deletes a specific notification for the current user.
    /// </summary>
    /// <param name="id">The ID of the notification to delete.</param>
    /// <returns>No content if successful, not found if the notification does not exist.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        var userId = GetCurrentUserId();
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (notification == null) return NotFound();

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}


