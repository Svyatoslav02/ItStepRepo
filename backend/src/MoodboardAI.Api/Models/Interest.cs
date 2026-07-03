using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents an interest category that users can select during onboarding.
/// </summary>
public class Interest
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Navigation property
    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
}
