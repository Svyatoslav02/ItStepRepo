using System;

namespace ItStepRepo.Services
{
    public interface IMoodboardService
    {
        MoodboardResponse GenerateMoodboard (string prompt);
    }
}
