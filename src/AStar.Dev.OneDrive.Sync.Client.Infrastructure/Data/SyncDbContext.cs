namespace AStar.Dev.OneDrive.Sync.Client.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts.Models;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration.Models;

/// <summary>
/// Entity Framework Core database context for OneDrive Sync persistent state.
/// </summary>
public class SyncDbContext(DbContextOptions<SyncDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets the table of OneDrive accounts configured for sync.
    /// </summary>
    public DbSet<SyncAccount> Accounts { get; init; }

    /// <summary>
    /// Gets the table of local filesystem roots designated for sync.
    /// </summary>
    public DbSet<SyncRoot> SyncRoots { get; init; }

    /// <summary>
    /// Gets the table of folder-level include/exclude selections.
    /// </summary>
    public DbSet<FolderSelection> FolderSelections { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SyncAccount>(entity =>
        {
            entity.HasKey(e => e.Email);
            entity.Property(e => e.Email).HasMaxLength(254).IsRequired();
            entity.Property(e => e.DisplayName).HasMaxLength(500);
            entity.Property(e => e.RefreshToken).HasMaxLength(2000);
            entity.Property(e => e.AccessToken).HasMaxLength(2000);
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            entity.Property(e => e.AddedAtUtc).IsRequired();
        });

        modelBuilder.Entity<SyncRoot>(entity =>
        {
            entity.HasKey(e => new { e.AccountEmail, e.LocalPath });
            entity.Property(e => e.AccountEmail).HasMaxLength(254).IsRequired();
            entity.Property(e => e.LocalPath).IsRequired();
            entity.Property(e => e.ConfiguredAtUtc).IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<FolderSelection>(entity =>
        {
            entity.HasKey(e => new { e.AccountEmail, e.FolderId });
            entity.Property(e => e.AccountEmail).HasMaxLength(254).IsRequired();
            entity.Property(e => e.FolderId).IsRequired();
            entity.Property(e => e.FolderPath).IsRequired();
            entity.Property(e => e.IsIncluded).HasDefaultValue(true);
            entity.Property(e => e.ModifiedAtUtc).IsRequired();
        });
    }
}
