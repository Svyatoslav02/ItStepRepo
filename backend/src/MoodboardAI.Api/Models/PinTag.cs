using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents the many-to-many relation between a <see cref="Pin"/> and a
/// <see cref="Tag"/> attached to it.
/// </summary>
/// <remarks>
/// A composite unique index on (<see cref="PinId"/>, <see cref="TagId"/>)
/// guarantees a pin cannot have the same tag attached more than once.
/// </remarks>
[Index(nameof(PinId), nameof(TagId), IsUnique = true)]
public class PinTag
{
    /// <summary>
    /// Unique identifier of the pin-tag relation record.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the tagged pin.
    /// </summary>
    [Required]
    public Guid PinId { get; set; }

    /// <summary>
    /// Navigation property to the related <see cref="Pin"/>.
    /// </summary>
    [ForeignKey(nameof(PinId))]
    public Pin Pin { get; set; } = null!;

    /// <summary>
    /// Identifier of the attached tag.
    /// </summary>
    [Required]
    public Guid TagId { get; set; }

    /// <summary>
    /// Navigation property to the related <see cref="Tag"/>.
    /// </summary>
    [ForeignKey(nameof(TagId))]
    public Tag Tag { get; set; } = null!;

    /// <summary>
    /// UTC timestamp when the tag was attached to the pin.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}