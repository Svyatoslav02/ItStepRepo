using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Extensions;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Services;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for generating moodboards.
/// </summary>
[ApiController]
[Route("api/generate")]
public class MoodboardController : ControllerBase
{
    private readonly IMoodboardService _moodboardService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MoodboardController"/> class.
    /// </summary>
    /// <param name="moodboardService">Moodboard generation service.</param>
    public MoodboardController(IMoodboardService moodboardService)
    {
        _moodboardService = moodboardService;
    }

    /// <summary>
    /// Generates a moodboard (real Unsplash photos, or mock data if no
    /// Unsplash key is configured) based on the provided prompt.
    /// </summary>
    /// <param name="request">Moodboard generation request.</param>
    /// <returns>Generated moodboard response.</returns>
    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] MoodboardRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.ToErrorResponse());
        }

        try
        {
            var response = await _moodboardService.GenerateAsync(request);
            return Ok(response);
        }
        catch (MoodboardGenerationException ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ErrorResponse { Message = ex.Message });
        }
    }
}