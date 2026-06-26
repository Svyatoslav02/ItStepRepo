using System;

namespace ItStepRepo.DTOs
{
    /// <summary>
    /// Represents an image in the moodboard.
    /// </summary>
    public class MoodboardImageDto
    {
        /// <summary>
        /// Gets or sets the URL of the image.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the image.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the source URL of the image.
        /// </summary>
        public string SourceUrl { get; set; } = string.Empty;
    }
}
