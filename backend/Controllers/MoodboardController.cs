using System;
using ItStepRepo.Services;
using ItStepRepo.DTOs;
using ItStepRepo.Models;

namespace ItStepRepo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodboardController : ControllerBase
    {
        private readonly IMoodboardService _service;

        public MoodboardController(IMoodboardService service)
        {
            _service = service;
        }

        /// <summary>
        /// Generates a moodboard based on the provided prompt.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Generated moodboard response</returns>
        [HttpPost("generate")]
        public IActionResult Generate([FromBody] GenerateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt is required.");

            var result = _service.GenerateMoodboard(request.Prompt);
            return Ok(result);
        }
    }
}
