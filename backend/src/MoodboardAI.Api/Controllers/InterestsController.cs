using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.DTOs.Interests;
using MoodboardAI.Api.Extensions;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for listing available onboarding
/// interests and saving the interests selected by the current user.
/// </summary>
[ApiController]
[Route("api")]
public class InterestsController : ControllerBase
{
    private readonly IInterestsService _interestsService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterestsController"/> class.
    /// </summary>
    /// <param name="interestsService">Interests service.</param>
    public InterestsController(IInterestsService interestsService)
    {
        _interestsService = interestsService;
    }

    /// <summary>
    /// Returns all interests available for selection during onboarding.
    /// </summary>
    /// <returns>List of available interests.</returns>
    [HttpGet("interests")]
    public async Task<IActionResult> GetAll()
    {
        var interests = await _interestsService.GetAllAsync();

        return Ok(interests);
    }

    /// <summary>
    /// Saves the interests selected by the currently authenticated user,
    /// replacing any previously saved selection.
    /// </summary>
    /// <param name="request">Request containing the selected interest ids. At least 3 are required.</param>
    /// <returns>The saved interests on success; 400 if the request is invalid or contains unknown interest ids.</returns>
    [Authorize]
    [HttpPost("users/me/interests")]
    public async Task<IActionResult> SaveMine([FromBody] SaveUserInterestsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErrorResponse());
        }

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new ErrorResponse { Message = "Invalid or missing authentication token." });
        }

        var result = await _interestsService.SaveUserInterestsAsync(userId.Value, request.InterestIds);

        if (!result.Succeeded)
        {
            return BadRequest(new ErrorResponse { Message = result.ErrorMessage ?? "Unable to save interests." });
        }

        return Ok(new SaveUserInterestsResponseDto { Interests = result.Interests! });
    }

    /// <summary>
    /// Extracts the authenticated user's id from the "sub" claim of the JWT.
    /// </summary>
    private Guid? GetCurrentUserId()
    {
        var subClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(subClaim, out var userId) ? userId : null;
    }
}
