using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MoodboardAI.Api.DTOs;

namespace MoodboardAI.Api.Controllers;
[ApiController]
[Route("api")]
public class MoodboardController : ControllerBase
{
    [HttpPost("generate")]
    public ActionResult<GenerateResponseDto> Generate([FromBody] GenerateRequestDto request)
    {
        var response = new GenerateResponseDto
        {
            Prompt = request.Prompt,
            Images = new List<ImageDto>
            {
                new ImageDto
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