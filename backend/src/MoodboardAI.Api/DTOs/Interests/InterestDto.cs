namespace MoodboardAI.Api.DTOs.Interests;

/// <summary>
/// Represents an interest available for selection during onboarding.
/// </summary>
public class InterestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}