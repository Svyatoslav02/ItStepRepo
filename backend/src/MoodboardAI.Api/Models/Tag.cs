using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a tag that can be attached to pins to support search and
/// filtering on the Home Feed and Search screens.
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public class Tag
{
    /// <summary>
    /// Unique identifier of the tag.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Display name of the tag (e.g. "minimalism", "sunset").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the tag record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}