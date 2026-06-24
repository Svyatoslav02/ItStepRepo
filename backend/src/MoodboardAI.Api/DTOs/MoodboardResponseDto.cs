using System;

namespace ItStepRepo.DTOs
{

    public class MoodboardResponseDto
    {
        public string Prompt { get; set; } = string.Empty;
        public List<MoodboardImageDto> Images { get; set; } = new List<MoodboardImageDto>();
    }
}
