using Microsoft.AspNetCore.Mvc;
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
    /// Generates a mock moodboard based on the provided prompt.
    /// </summary>
    /// <param name="request">Moodboard generation request.</param>
    /// <returns>Generated mock moodboard response.</returns>
    [HttpPost]
    public IActionResult Generate([FromBody] MoodboardRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage)
                .FirstOrDefault() ?? "Invalid request.";

            return BadRequest(new ErrorResponse
            {
                Message = errorMessage
            });
        }

        var response = _moodboardService.Generate(request);

        return Ok(response);
    }
}