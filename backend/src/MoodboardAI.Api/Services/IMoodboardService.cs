using System;

namespace ItStepRepo.Services
{
    /// <summary>
    /// Defines the contract for a service that generates 
    /// moodboards based on textual prompts.
    /// </summary>
    public interface IMoodboardService
    {
        MoodboardResponse GenerateMoodboard (string prompt);
    }
}
