using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Data
{
    /// <summary>
    /// Here must by bd
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        
        
        /// <summary>
        /// Seed date about interest
        /// </summary>
        public DbSet<Interest> Interests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Interest>().HasData(
                new Interest { Id = 1, Name = "Minimal" },
                new Interest { Id = 2, Name = "3D-искусство" },
                new Interest { Id = 3, Name = "Мобильные приложения" },
                new Interest { Id = 4, Name = "Ретро-арт" },
                new Interest { Id = 5, Name = "Фотография" },
                new Interest { Id = 6, Name = "Архитектура" },
                new Interest { Id = 7, Name = "Современность" },
                new Interest { Id = 8, Name = "Искусство" },
                new Interest { Id = 9, Name = "Эко-арт" },
                new Interest { Id = 10, Name = "Печатная продукция" }
            );
        }
    }
}