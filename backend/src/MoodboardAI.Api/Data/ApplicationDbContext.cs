using Microsoft.EntityFrameworkCore;
using ItStepRepo.Models; 

namespace ItStepRepo.Data
{
    /// <summary>
    /// Represents the application's database context, 
    /// providing access to the database and managing entity sets.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the 
        /// specified options.
        /// </summary>
        /// <param name="options">The options for the context.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{User}"/> representing 
        /// the Users table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}

