using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.DTOs.Interests;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for retrieving available interests
/// and saving the current user's selected interests.
/// </summary>
[ApiController]
[Route("api")]
public class InterestsController : ControllerBase
{
    private readonly IInterestService _interestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterestsController"/> class.
    /// </summary>
    /// <param name="interestService">Interest management service.</param>
    public InterestsController(IInterestService interestService)
    {
        _interestService = interestService;
    }

    /// <summary>
    /// Returns all available interests.
    /// </summary>
    /// <returns>List of available interests.</returns>
    [HttpGet("interests")]
    public IActionResult GetInterests()
    {
        var interests = _interestService.GetAllInterests();
        return Ok(interests);
    }

    /// <summary>
    /// Saves the selected interests for the current authenticated user.
    /// </summary>
    /// <param name="request">Selected interest identifiers.</param>
    /// <returns>No content on success, or an error response on validation failure.</returns>
    [HttpPost("users/me/interests")]
    [Authorize]
    public IActionResult SaveMyInterests([FromBody] SaveInterestsRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Invalid request.";

            return BadRequest(new ErrorResponse { Message = errorMessage });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ErrorResponse { Message = "User is not authenticated." });
        }

        var (success, saveErrorMessage) = _interestService.SaveUserInterests(userId, request.InterestIds);

        if (!success)
        {
            return BadRequest(new ErrorResponse { Message = saveErrorMessage ?? "Invalid request." });
        }

        return NoContent();
    }
}