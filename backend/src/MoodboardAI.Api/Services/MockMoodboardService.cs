using System;
using System.Collections.Generic;
using ItStepRepo.DTOs;

namespace ItStepRepo.Services
{
    /// <summary>
    /// Mock implementation of the IMoodboardService interface.
    /// </summary>
    public class MockMoodboardService : IMoodboardService
    {
        /// <summary>
        /// Generates a moodboard based on the provided prompt. 
        /// This is a mock implementation that returns hardcoded images 
        /// for demonstration purposes.
        /// </summary>
        /// <param name="prompt">The textual prompt for generating the moodboard.</param>
        /// <returns>Generated moodboard response</returns>
        /// <exception cref="ArgumentException">Thrown when the prompt is empty or null.</exception>
	    public MoodboardResponse GenerateMoodboard(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentException("Prompt cannot be empty.", nameof(prompt));
            }
        
            var response = new MoodboardResponseDto();
            {
                Prompt = prompt

                Images = new List<MoodboardImageDto>
                {
                    new MoodboardImageDto
                    {
                        Url = "https://example.com/image1.jpg",
                        Title = "Image 1",
                        SourceUrl = "https://example.com/source1"
                    },
                    new MoodboardImageDto
                    {
                        Url = "https://example.com/image2.jpg",
                        Title = "Image 2",
                        SourceUrl = "https://example.com/source2"
                    },
                    new MoodboardImageDto
                    {
                        Url = "https://example.com/image3.jpg",
                        Title = "Image 3",
                        SourceUrl = "https://example.com/source3"
                    }
                }
            };
            return response;
        }
    }
}
