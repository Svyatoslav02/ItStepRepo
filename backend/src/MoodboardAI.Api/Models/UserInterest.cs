using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents the relation between a user and a selected interest.
/// Stores which interests the user selected during onboarding.
/// Duplicate user-interest pairs are prevented via a composite unique key.
/// </summary>
public class UserInterest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid InterestId { get; set; }

    // Navigation properties
    public UserEntity User { get; set; } = null!;
    public Interest Interest { get; set; } = null!;
}
