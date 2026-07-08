using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoodboardAI.Api.Models;

/// <summary>
/// Represents the relation between a <see cref="UserEntity"/> and an
/// <see cref="Interest"/> the user selected during onboarding.
/// </summary>
/// <remarks>
/// A composite unique index on (<see cref="UserId"/>, <see cref="InterestId"/>)
/// guarantees a user cannot select the same interest more than once.
/// </remarks>
[Index(nameof(UserId), nameof(InterestId), IsUnique = true)]
public class UserInterest
{
    /// <summary>
    /// Unique identifier of the user-interest relation record.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Identifier of the user who selected the interest.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property to the related <see cref="UserEntity"/>.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; } = null!;

    /// <summary>
    /// Identifier of the selected interest.
    /// </summary>
    [Required]
    public Guid InterestId { get; set; }

    /// <summary>
    /// Navigation property to the related <see cref="Interest"/>.
    /// </summary>
    [ForeignKey(nameof(InterestId))]
    public Interest Interest { get; set; } = null!;

    /// <summary>
    /// UTC timestamp when the user selected this interest.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
