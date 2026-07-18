using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a category used to classify pins shown in the Home Feed and
/// Search screens (e.g. "Interior", "Fashion").
/// </summary>
[Index(nameof(Name), IsUnique = true)]
public class Category
{
    /// <summary>
    /// Unique identifier of the category.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Display name of the category.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Icon identifier representing the category in the UI.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the category record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}