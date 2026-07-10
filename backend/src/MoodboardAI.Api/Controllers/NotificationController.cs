using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Extensions;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Dtos.Notification;
using Microsoft.AspNetCore.Authorization;
using MoodboardAI.Api.Data;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for managing user notifications.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    /// <summary>
    /// The database context used to access the application's data.
    /// </summary>
    private readonly ApplicationDbContext _context;
    /// <summary>
    /// The user manager used to manage user accounts and retrieve user information.
    /// </summary>
    private readonly UserManager<ApplicationUser> _userManager;

    public NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET /api/notifications?page=1&pageSize=10
    /// <summary>
    /// Retrieves a paginated list of notifications for the authenticated user.
    /// </summary>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="pageSize">The number of notifications to retrieve per page.</param>
    /// <returns>A list of notifications for the authenticated user.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(int page = 1, int pageSize = 10)
    {
        var userId = _userManager.GetUserId(User);

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

    // PUT /api/notifications/{id}/read
    /// <summary>
    /// Marks a specific notification as read for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the notification to mark as read.</param>
    /// <returns>No content if the notification is successfully marked as read.</returns>
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = _userManager.GetUserId(User);
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (notification == null) return NotFound();

        notification.IsRead = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT /api/notifications/read-all
    /// <summary>
    /// Marks all notifications as read for the authenticated user.
    /// </summary>
    /// <returns>No content if all notifications are successfully marked as read.</returns>
    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = _userManager.GetUserId(User);

        var notifications = await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
        foreach (var n in notifications) n.IsRead = true;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/notifications/{id}
    /// <summary>
    /// Deletes a specific notification for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the notification to delete.</param>
    /// <returns>No content if the notification is successfully deleted.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        var userId = _userManager.GetUserId(User);
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

        if (notification == null) return NotFound();

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

