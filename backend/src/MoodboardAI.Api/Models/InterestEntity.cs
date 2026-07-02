using System;
using System.ComponentModel.DataAnnotations;

namespace MoodboardAI.Api.Models
{
    /// <summary>
    /// Represents an interest entity in the application.
    /// </summary>
    public class InterestEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the interest entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Gets or sets the name of the interest entity.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the icon for the interest entity.
        /// </summary>
        [MaxLength(200)]
        public string Icon { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the creation date and time for the interest entity.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}


