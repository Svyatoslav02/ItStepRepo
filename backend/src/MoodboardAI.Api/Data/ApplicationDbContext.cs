using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Models;
using MoodboardAI.Api.Models.Entities;

namespace MoodboardAI.Api.Data;

/// <summary>
/// Entity Framework Core database context for the MoodboardAI application.
/// Backed by PostgreSQL (Supabase).
/// </summary>
/// <remarks>
/// NOTE: This context did not previously exist in the project. It is added here
/// as a required dependency of the "Create Interest entity model" and
/// "Create Auth service" tasks, which explicitly require an ApplicationDbContext
/// to register the Interest entity and to look up/persist users. Full EF Core +
/// Supabase configuration (connection string wiring, initial migration generation)
/// remains tracked separately.
/// </remarks>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">EF Core context options (configured in Program.cs).</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Application users.
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();

    /// <summary>
    /// Onboarding interests available for users to select.
    /// </summary>
    public DbSet<Interest> Interests => Set<Interest>();

    /// <summary>
    /// Relation records linking users to the interests they selected during onboarding.
    /// </summary>
    public DbSet<UserInterest> UserInterests => Set<UserInterest>();
    
    /// <summary>
    /// Categories grouping pins
    /// </summary>
    public DbSet<Category> Categories => Set<Category>();
    
    /// <summary>
    /// Tags that can be attached to pins for flexible filtering.
    /// </summary>
    public DbSet<Tag> Tags => Set<Tag>();
    /// <summary>
    /// Pins represent saved items (images, inspirations, etc.).
    /// </summary>
    public DbSet<Pin> Pins => Set<Pin>();
    /// <summary>
    /// Many-to-many relation between pins and tags.
    /// </summary>
    public DbSet<PinTag> PinTags => Set<PinTag>();
    

    /// <summary>
    /// Privacy settings for each user.
    /// </summary>
    public DbSet<UserPrivacySettings> UserPrivacySettings => Set<UserPrivacySettings>();

    /// <summary>
    /// Block relations between users.
    /// </summary>
    public DbSet<BlockedUser> BlockedUsers => Set<BlockedUser>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Interest>().HasData(InterestSeedData.Default);

        modelBuilder.Entity<UserInterest>(entity =>
        {
            // Composite unique key: a user can select a given interest only once.
            // Declared explicitly here (in addition to the [Index] attribute on the
            // entity) so the constraint is unambiguous regardless of attribute discovery.
            entity.HasIndex(userInterest => new { userInterest.UserId, userInterest.InterestId })
                .IsUnique();

            entity.HasOne(userInterest => userInterest.User)
                .WithMany()
                .HasForeignKey(userInterest => userInterest.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(userInterest => userInterest.Interest)
                .WithMany()
                .HasForeignKey(userInterest => userInterest.InterestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // UserPrivacySettings: one-to-one with UserEntity
        modelBuilder.Entity<UserPrivacySettings>(entity =>
        {
            entity.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<UserPrivacySettings>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // BlockedUser: composite unique key prevents duplicate blocks
        modelBuilder.Entity<BlockedUser>(entity =>
        {
            entity.HasIndex(b => new { b.BlockerId, b.BlockedUserId })
                .IsUnique();

            entity.HasOne(b => b.Blocker)
                .WithMany()
                .HasForeignKey(b => b.BlockerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Blocked)
                .WithMany()
                .HasForeignKey(b => b.BlockedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        //Creating initial data for tags
        TegsSeedData.Seed(modelBuilder);
        //Creating links for tags
        modelBuilder.Entity<PinTag>()
            .HasKey(pt => new { pt.PinId, pt.TagId });

        modelBuilder.Entity<Pin>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Pins)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PinTag>()
            .HasOne(pt => pt.Pin)
            .WithMany(p => p.PinTags)
            .HasForeignKey(pt => pt.PinId);

        modelBuilder.Entity<PinTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PinTags)
            .HasForeignKey(pt => pt.TagId);
    }
}
