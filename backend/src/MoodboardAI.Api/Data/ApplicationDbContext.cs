using Microsoft.EntityFrameworkCore;
using MoodboardAI.Api.Models;

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
    /// Pins shown in the Home Feed and Search screens.
    /// </summary>
    public DbSet<Pin> Pins => Set<Pin>();

    /// <summary>
    /// Categories used to classify pins.
    /// </summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>
    /// Tags that can be attached to pins.
    /// </summary>
    public DbSet<Tag> Tags => Set<Tag>();

    /// <summary>
    /// Relation records linking pins to the tags attached to them.
    /// </summary>
    public DbSet<PinTag> PinTags => Set<PinTag>();
    /// Privacy settings for each user.
    /// </summary>
    public DbSet<UserPrivacySettings> UserPrivacySettings => Set<UserPrivacySettings>();

    /// <summary>
    /// Block relations between users.
    /// </summary>
    public DbSet<BlockedUser> BlockedUsers => Set<BlockedUser>();
    /// <summary>
    /// Stores user likes on pins.
    /// Each record represents that a specific user has liked a specific pin.
    /// </summary>
    public DbSet<Like> Likes => Set<Like>();
    /// <summary>
    /// Stores user saves of pins.
    /// Each record represents that a specific user has saved a specific pin to their collection.
    /// </summary>
    public DbSet<Save> Saves => Set<Save>();
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Interest>().HasData(InterestSeedData.Default);

        modelBuilder.Entity<UserInterest>(entity =>
        {
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

        modelBuilder.Entity<Pin>(entity =>
        {
            entity.HasOne(pin => pin.Author)
                .WithMany()
                .HasForeignKey(pin => pin.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pin => pin.Category)
                .WithMany()
                .HasForeignKey(pin => pin.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PinTag>(entity =>
        {
            entity.HasIndex(pinTag => new { pinTag.PinId, pinTag.TagId })
                .IsUnique();

            entity.HasOne(pinTag => pinTag.Pin)
                .WithMany(pin => pin.PinTags)
                .HasForeignKey(pinTag => pinTag.PinId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pinTag => pinTag.Tag)
                .WithMany()
                .HasForeignKey(pinTag => pinTag.TagId)
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
        
        // Like: зв’язок Pin ↔ User
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasIndex(l => new { l.PinId, l.UserId }).IsUnique();

            entity.HasOne(l => l.Pin)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PinId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

// Save: зв’язок Pin ↔ User
        modelBuilder.Entity<Save>(entity =>
        {
            entity.HasIndex(s => new { s.PinId, s.UserId }).IsUnique();

            entity.HasOne(s => s.Pin)
                .WithMany(p => p.Saves)
                .HasForeignKey(s => s.PinId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.User)
                .WithMany(u => u.Saves)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        
    }
}