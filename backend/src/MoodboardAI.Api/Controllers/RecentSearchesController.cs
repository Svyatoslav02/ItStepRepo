using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Data;
using MoodboardAI.Api.DTOs.Search;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// Manages recent search queries for the current authenticated user.
/// </summary>
[ApiController]
[Route("api/users/me/recent-searches")]
[Authorize]
public class RecentSearchesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public RecentSearchesController(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns the current user's recent searches, newest first.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRecentSearches()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var searches = await _db.RecentSearches
            .Where(r => r.UserId == userId.Value)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new RecentSearchDto
            {
                Id = r.Id,
                Query = r.Query,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(searches);
    }

    /// <summary>
    /// Saves a search query for the current user.
    /// If the same query already exists, updates its CreatedAt instead of creating a duplicate.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddRecentSearch([FromBody] AddRecentSearchRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(Error(ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Invalid request."));

        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var existing = await _db.RecentSearches
            .FirstOrDefaultAsync(r => r.UserId == userId.Value && r.Query == request.Query);

        if (existing != null)
        {
            // Update timestamp instead of creating a duplicate
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            _db.RecentSearches.Add(new RecentSearch
            {
                UserId = userId.Value,
                Query = request.Query
            });
        }

        await _db.SaveChangesAsync();

        return Ok(new { message = "Search saved." });
    }

    /// <summary>
    /// Clears all recent searches for the current user.
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> ClearRecentSearches()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(Error("Invalid token."));

        var searches = await _db.RecentSearches
            .Where(r => r.UserId == userId.Value)
            .ToListAsync();

        _db.RecentSearches.RemoveRange(searches);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Recent searches cleared." });
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
