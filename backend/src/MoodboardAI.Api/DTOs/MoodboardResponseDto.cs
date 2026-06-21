using System;

namespace ItStepRepo.DTOs
{
    /// <summary>
    /// Represents the response shape for 
    /// a moodboard generation request.
    /// It contains the original prompt and 
    /// a list of images that make up the moodboard.
    /// </summary>
    public class MoodboardResponseDto
    {
        public string Prompt { get; set; } = string.Empty;
        public List<MoodboardImageDto> Images { get; set; } = new List<MoodboardImageDto>();
    }
}
