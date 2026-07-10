using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents a user's notification preferences 
/// for different channels and types of notifications.
/// </summary>
public class NotificationPreference
{
    /// <summary>
    /// Unique identifier of the notification preference record.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    /// <summary>
    /// Identifier of the user to whom these notification preferences belong.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
    /// <summary>
    /// Navigation property to the related <see cref="UserEntity"/>.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; } = null!;

    // Channel preferences
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for likes.
    /// </summary>
    public bool PushLikes { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for comments.
    /// </summary>
    public bool PushComments { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for tags.
    /// </summary>
    public bool PushTags { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for friend requests.
    /// </summary>
    public bool PushFriendRequests { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for updates.
    /// </summary>
    public bool PushUpdates { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for recommendations.
    /// </summary>
    public bool PushRecommendations { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications for mentions.
    /// </summary>
    public bool PushMentions { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for likes.
    /// </summary>
    public bool EmailLikes { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for comments.
    /// </summary>
    public bool EmailComments { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for tags.
    /// </summary>
    public bool EmailTags { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for friend requests.
    /// </summary>
    public bool EmailFriendRequests { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for updates.
    /// </summary>
    public bool EmailUpdates { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for recommendations.
    /// </summary>
    public bool EmailRecommendations { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications for mentions.
    /// </summary>
    public bool EmailMentions { get; set; }

    // Quiet mode
    /// <summary>
    /// Indicates whether the user has enabled quiet mode, which suppresses all notifications.
    /// </summary>
    public bool QuietMode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
