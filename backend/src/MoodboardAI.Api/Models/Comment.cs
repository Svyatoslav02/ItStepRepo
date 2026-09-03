using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a comment left by a user on a <see cref="Pin"/>, shown in
/// the Home Feed pin detail view (see Image 6 — "Comments" section).
/// </summary>
[Index(nameof(PinId), nameof(CreatedAt))]
public class Comment
{
    /// <summary>
    /// Unique identifier of the comment.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the pin this comment was posted on.
    /// </summary>
    [Required]
    public Guid PinId { get; set; }

    /// <summary>
    /// Navigation property to the commented pin.
    /// </summary>
    [ForeignKey(nameof(PinId))]
    public Pin Pin { get; set; } = null!;

    /// <summary>
    /// Identifier of the user who wrote the comment.
    /// </summary>
    [Required]
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Navigation property to the comment's author.
    /// </summary>
    [ForeignKey(nameof(AuthorId))]
    public UserEntity Author { get; set; } = null!;

    /// <summary>
    /// Text content of the comment.
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the comment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}