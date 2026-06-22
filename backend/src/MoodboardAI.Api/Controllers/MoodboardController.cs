using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// API controller that exposes endpoints for generating moodboards.
/// </summary>
public class MoodboardController : ControllerBase
{
    /// <summary>
    /// Generates a moodboard based on the given prompt.
    /// </summary>
    [HttpPost("generate")]
    public IActionResult Generate([FromBody] MoodboardRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = ModelState
                .SelectMany(entry => entry.Value?.Errors ?? new())
                .Select(error => error.ErrorMessage)
                .FirstOrDefault() ?? "Invalid request.";

            return BadRequest(new ErrorResponse
            {
                Message = errorMessage
            });
        }

        var response = new MoodboardResponse
        {
            Prompt = request.Prompt,
            Images = new List<string>()
        };

        return Ok(response);
    }
}
