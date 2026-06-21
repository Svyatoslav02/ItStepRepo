using System;

namespace ItStepRepo.DTOs
{
    /// <summary>
    /// Represents an image in a moodboard, 
    /// including its URL, title, and source URL.
    /// </summary>
    public class MoodboardImageDto
    {
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
    }
}
