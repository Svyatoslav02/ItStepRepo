using System;
using System.ComponentModel.DataAnnotations;

namespace ItStepRepo.Models
{
    public class User
    {
        [Key] // Первинний ключ
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

