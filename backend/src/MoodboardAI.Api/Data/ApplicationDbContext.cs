using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Data;

/// <summary>
/// The main EF Core database context for the MoodboardAI application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<Interest> Interests => Set<Interest>();
    public DbSet<UserInterest> UserInterests => Set<UserInterest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserInterest>(entity =>
        {
            // Composite primary key — also prevents duplicate user-interest records
            entity.HasKey(ui => new { ui.UserId, ui.InterestId });

            // User relation
            entity.HasOne(ui => ui.User)
                  .WithMany(u => u.UserInterests)
                  .HasForeignKey(ui => ui.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Interest relation
            entity.HasOne(ui => ui.Interest)
                  .WithMany(i => i.UserInterests)
                  .HasForeignKey(ui => ui.InterestId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
