using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.DTOs.Notification;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;
using MoodboardAI.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MoodboardAI.Api.Extensions;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for managing user notification preferences.
/// </summary>
[ApiController]
[Route("api/users/me/notification-preferences")]
[Authorize]
public class NotificationPreferencesController : ControllerBase
{
    /// <summary>
    /// The database context for accessing notification preferences.
    /// </summary>
    private readonly ApplicationDbContext _context;
    /// <summary>
    /// The user service for retrieving the current user's ID.
    /// </summary>
    private readonly IUserService _userService;

    public NotificationPreferencesController(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    /// <summary>
    /// Gets the notification preferences for the currently authenticated user. 
    /// If no preferences exist, default preferences are created and returned.
    /// </summary>
    /// <returns>The notification preferences for the current user.</returns>
    [HttpGet]
    public async Task<ActionResult<NotificationPreferenceDto>> GetPreferences()
    {
        var userId = _userService.GetCurrentUserId();
        var prefs = await _context.NotificationPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (prefs == null)
        {
            prefs = new NotificationPreference { UserId = userId };
            _context.NotificationPreferences.Add(prefs);
            await _context.SaveChangesAsync();
        }

        return Ok(MapToDto(prefs));
    }

    /// <summary>
    /// Updates the notification preferences for the currently authenticated user.
    /// </summary>
    /// <param name="dto">The notification preference data to update.</param>
    /// <returns>The updated notification preferences.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdatePreferences([FromBody] NotificationPreferenceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ToErrorResponse());

        var userId = _userService.GetCurrentUserId();
        var prefs = await _context.NotificationPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (prefs == null)
        {
            prefs = new NotificationPreference { UserId = userId };
            _context.NotificationPreferences.Add(prefs);
        }

        MapFromDto(prefs, dto);
        await _context.SaveChangesAsync();

        return Ok(MapToDto(prefs));
    }

    /// <summary>
    /// Maps a NotificationPreference entity to a NotificationPreferenceDto.
    /// </summary>
    /// <param name="prefs">The NotificationPreference entity to map.</param>
    /// <returns>The mapped NotificationPreferenceDto.</returns>
    private NotificationPreferenceDto MapToDto(NotificationPreference prefs) =>
        new NotificationPreferenceDto
        {
            PushLikes = prefs.PushLikes,
            PushComments = prefs.PushComments,
            PushTags = prefs.PushTags,
            PushFriendRequests = prefs.PushFriendRequests,
            PushUpdates = prefs.PushUpdates,
            PushRecommendations = prefs.PushRecommendations,
            PushMentions = prefs.PushMentions,
            EmailLikes = prefs.EmailLikes,
            EmailComments = prefs.EmailComments,
            EmailTags = prefs.EmailTags,
            EmailFriendRequests = prefs.EmailFriendRequests,
            EmailUpdates = prefs.EmailUpdates,
            EmailRecommendations = prefs.EmailRecommendations,
            EmailMentions = prefs.EmailMentions,
            QuietMode = prefs.QuietMode
        };

    /// <summary>
    /// Maps a NotificationPreferenceDto to a NotificationPreference entity, 
    /// updating only the properties that are not null in the DTO.
    /// </summary>
    /// <param name="prefs">The NotificationPreference entity to map.</param>
    /// <param name="dto">The NotificationPreferenceDto to map from.</param>
    private void MapFromDto(NotificationPreference prefs, NotificationPreferenceDto dto)
    {
        if (dto.PushLikes.HasValue) prefs.PushLikes = dto.PushLikes.Value;
        if (dto.PushComments.HasValue) prefs.PushComments = dto.PushComments.Value;
        if (dto.PushTags.HasValue) prefs.PushTags = dto.PushTags.Value;
        if (dto.PushFriendRequests.HasValue) prefs.PushFriendRequests = dto.PushFriendRequests.Value;
        if (dto.PushUpdates.HasValue) prefs.PushUpdates = dto.PushUpdates.Value;
        if (dto.PushRecommendations.HasValue) prefs.PushRecommendations = dto.PushRecommendations.Value;
        if (dto.PushMentions.HasValue) prefs.PushMentions = dto.PushMentions.Value;

        if (dto.EmailLikes.HasValue) prefs.EmailLikes = dto.EmailLikes.Value;
        if (dto.EmailComments.HasValue) prefs.EmailComments = dto.EmailComments.Value;
        if (dto.EmailTags.HasValue) prefs.EmailTags = dto.EmailTags.Value;
        if (dto.EmailFriendRequests.HasValue) prefs.EmailFriendRequests = dto.EmailFriendRequests.Value;
        if (dto.EmailUpdates.HasValue) prefs.EmailUpdates = dto.EmailUpdates.Value;
        if (dto.EmailRecommendations.HasValue) prefs.EmailRecommendations = dto.EmailRecommendations.Value;
        if (dto.EmailMentions.HasValue) prefs.EmailMentions = dto.EmailMentions.Value;

        if (dto.QuietMode.HasValue) prefs.QuietMode = dto.QuietMode.Value;

        prefs.UpdatedAt = DateTime.UtcNow;
    }
}