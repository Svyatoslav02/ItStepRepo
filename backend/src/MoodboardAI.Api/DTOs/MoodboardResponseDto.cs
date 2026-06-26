using System;

namespace ItStepRepo.DTOs
{
    /// <summary>
    /// Represents the response for a moodboard request.
    /// </summary>
    public class MoodboardResponseDto
    {
        /// <summary>
        /// Gets or sets the prompt used to generate the moodboard.
        /// </summary>
        public string Prompt { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of images in the moodboard.
        /// </summary>
        public List<MoodboardImageDto> Images { get; set; } = new List<MoodboardImageDto>();
    }
}
