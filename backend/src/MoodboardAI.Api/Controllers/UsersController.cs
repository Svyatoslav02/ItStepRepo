using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// Exposes endpoints for retrieving the current authenticated user's profile.
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">User profile service.</param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets the profile of the currently authenticated user.
    /// </summary>
    /// <returns>Profile data: id, full name, email, avatar, selected interests, onboarding status.</returns>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMe()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponse
            {
                Message = "Invalid or missing authentication token."
            });
        }

        var profile = _userService.GetCurrentUser(userId);

        if (profile is null)
        {
            return NotFound(new ErrorResponse
            {
                Message = "User not found."
            });
        }

        return Ok(profile);
    }

    /// <summary>
    /// Extracts the authenticated user's id from the "sub" claim of the JWT.
    /// Falls back to <see cref="ClaimTypes.NameIdentifier"/> in case inbound
    /// claim mapping is ever re-enabled.
    /// </summary>
    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}