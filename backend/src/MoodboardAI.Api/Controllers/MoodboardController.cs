using Microsoft.AspNetCore.Mvc;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Controllers;

/// <summary>
/// API controller that exposes endpoints for generating moodboards.
/// </summary>
[ApiController]
[Route("api/generate")]
public class MoodboardController : ControllerBase
{
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

        var response = new MoodboardResponse
        {
            Prompt = request.Prompt,
            Images = new List<MoodboardImage>
            {
                new()
                {
                    Url = "https://example.com/image1.jpg",
                    Title = "Minimalist interior",
                    SourceUrl = "https://example.com"
                }
            }
        };

        return Ok(response);
    }
}