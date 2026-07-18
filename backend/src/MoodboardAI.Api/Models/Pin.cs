using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a pin — a single piece of visual content shown in the Home
/// Feed and Search screens.
/// </summary>
[Index(nameof(Title))]
[Index(nameof(AuthorId))]
[Index(nameof(CategoryId))]
public class Pin
{
    /// <summary>
    /// Unique identifier of the pin.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Title of the pin.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional longer description of the pin.
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// URL of the pin's image.
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Optional URL of the original source of the image.
    /// </summary>
    [MaxLength(1000)]
    public string? SourceUrl { get; set; }

    /// <summary>
    /// Identifier of the user who authored/uploaded the pin.
    /// </summary>
    [Required]
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Navigation property to the pin's author.
    /// </summary>
    [ForeignKey(nameof(AuthorId))]
    public UserEntity Author { get; set; } = null!;

    /// <summary>
    /// Identifier of the category this pin belongs to.
    /// </summary>
    [Required]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Navigation property to the pin's category.
    /// </summary>
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Relation records linking this pin to its tags.
    /// </summary>
    public ICollection<PinTag> PinTags { get; set; } = new List<PinTag>();

    /// <summary>
    /// UTC timestamp when the pin was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp when the pin was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}