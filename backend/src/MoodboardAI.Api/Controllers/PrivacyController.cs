using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Privacy;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// Manages user privacy settings, blocked users, and data export requests.
/// All endpoints require authentication.
/// </summary>
[ApiController]
[Route("api/users/me")]
[Authorize]
public class PrivacyController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public PrivacyController(ApplicationDbContext db)
    {
        _db = db;
    }

    // ──────────────────────────────────────────────
    // Privacy settings
    // ──────────────────────────────────────────────

    /// <summary>
    /// Returns the current user's privacy settings.
    /// </summary>
    [HttpGet("privacy")]
    public async Task<IActionResult> GetPrivacySettings()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var settings = await _db.UserPrivacySettings
            .FirstOrDefaultAsync(p => p.UserId == userId.Value);

        if (settings == null)
        {
            // Return defaults if not yet configured
            return Ok(new PrivacySettingsDto
            {
                PrivateAccount = false,
                SearchVisibility = true,
                ContentVisibility = true
            });
        }

        return Ok(new PrivacySettingsDto
        {
            PrivateAccount = settings.PrivateAccount,
            SearchVisibility = settings.SearchVisibility,
            ContentVisibility = settings.ContentVisibility
        });
    }

    /// <summary>
    /// Updates the current user's privacy settings.
    /// </summary>
    [HttpPut("privacy")]
    public async Task<IActionResult> UpdatePrivacySettings([FromBody] UpdatePrivacySettingsDto request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var settings = await _db.UserPrivacySettings
            .FirstOrDefaultAsync(p => p.UserId == userId.Value);

        if (settings == null)
        {
            settings = new UserPrivacySettings { UserId = userId.Value };
            _db.UserPrivacySettings.Add(settings);
        }

        settings.PrivateAccount = request.PrivateAccount;
        settings.SearchVisibility = request.SearchVisibility;
        settings.ContentVisibility = request.ContentVisibility;

        await _db.SaveChangesAsync();

        return Ok(new PrivacySettingsDto
        {
            PrivateAccount = settings.PrivateAccount,
            SearchVisibility = settings.SearchVisibility,
            ContentVisibility = settings.ContentVisibility
        });
    }

    // ──────────────────────────────────────────────
    // Blocked users
    // ──────────────────────────────────────────────

    /// <summary>
    /// Returns the list of users blocked by the current user.
    /// </summary>
    [HttpGet("blocked-users")]
    public async Task<IActionResult> GetBlockedUsers()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var blocked = await _db.BlockedUsers
            .Where(b => b.BlockerId == userId.Value)
            .Include(b => b.Blocked)
            .Select(b => new BlockedUserDto
            {
                BlockedUserId = b.BlockedUserId,
                Username = b.Blocked.Username,
                BlockedAt = b.CreatedAt
            })
            .ToListAsync();

        return Ok(blocked);
    }

    /// <summary>
    /// Blocks the specified user.
    /// Returns 400 if the user tries to block themselves or the user is already blocked.
    /// </summary>
    [HttpPost("blocked-users")]
    public async Task<IActionResult> BlockUser([FromBody] BlockUserRequestDto request)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        // User cannot block themselves
        if (userId.Value == request.UserId)
            return BadRequest(Error("You cannot block yourself."));

        // Prevent duplicate blocks
        var alreadyBlocked = await _db.BlockedUsers
            .AnyAsync(b => b.BlockerId == userId.Value && b.BlockedUserId == request.UserId);

        if (alreadyBlocked)
            return BadRequest(Error("This user is already blocked."));

        // Verify target user exists
        var targetExists = await _db.Users.AnyAsync(u => u.Id == request.UserId);
        if (!targetExists)
            return NotFound(Error("User not found."));

        _db.BlockedUsers.Add(new BlockedUser
        {
            BlockerId = userId.Value,
            BlockedUserId = request.UserId
        });

        await _db.SaveChangesAsync();

        return Ok(new { message = "User blocked successfully." });
    }

    /// <summary>
    /// Unblocks the specified user.
    /// </summary>
    [HttpDelete("blocked-users/{blockedUserId:guid}")]
    public async Task<IActionResult> UnblockUser(Guid blockedUserId)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var record = await _db.BlockedUsers
            .FirstOrDefaultAsync(b => b.BlockerId == userId.Value && b.BlockedUserId == blockedUserId);

        if (record == null)
            return NotFound(Error("Blocked user record not found."));

        _db.BlockedUsers.Remove(record);
        await _db.SaveChangesAsync();

        return Ok(new { message = "User unblocked successfully." });
    }

    // ──────────────────────────────────────────────
    // Data export
    // ──────────────────────────────────────────────

    /// <summary>
    /// Placeholder endpoint for requesting a data export.
    /// The actual export logic will be implemented separately.
    /// </summary>
    [HttpPost("data-export")]
    public IActionResult RequestDataExport()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        // TODO: implement actual data export logic
        return Ok(new { message = "Data export request received. You will be notified when your export is ready." });
    }

    // ──────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────

    private Guid? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                 ?? User.FindFirst("sub");

        return claim != null && Guid.TryParse(claim.Value, out var id) ? id : null;
    }

    private static ErrorResponse Error(string message) => new() { Message = message };
}
