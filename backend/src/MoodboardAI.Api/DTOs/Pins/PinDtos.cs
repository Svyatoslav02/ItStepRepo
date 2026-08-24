using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.DTOs.Pins;

/// <summary>
/// Request DTO for creating a new pin.
/// </summary>
public class CreatePinRequestDto
{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Image URL is required.")]
    [MaxLength(1000)]
    [Url(ErrorMessage = "Image URL must be a valid URL.")]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? SourceUrl { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Tag names to attach to the pin. New tags are created on the fly.
    /// </summary>
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// Response DTO returned after successfully creating a pin.
/// </summary>
public class PinResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? SourceUrl { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Request DTO for posting a comment on a pin.
/// </summary>
public class CreateCommentRequestDto
{
    [Required(ErrorMessage = "Comment text is required.")]
    [MinLength(1, ErrorMessage = "Comment cannot be empty.")]
    [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO representing a single comment on a pin.
/// </summary>
public class CommentDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string AuthorUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}