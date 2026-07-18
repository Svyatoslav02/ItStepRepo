using System.ComponentModel.DataAnnotations;
using MoodboardAI.Api.DTOs.Interests;

namespace MoodboardAI.Api.DTOs.Users;

/// <summary>
/// Profile of the currently authenticated user: basic data,
/// onboarding status, and selected interests.
/// </summary>
public class UserProfileDto
{
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public List<InterestDto> SelectedInterests { get; set; } = new();
    public bool IsOnboardingCompleted { get; set; }
}