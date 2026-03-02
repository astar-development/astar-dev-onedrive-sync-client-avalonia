namespace AStar.Dev.OneDrive.Sync.Client.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration.Models;
using Data;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class SyncConfigurationRepository(SyncDbContext dbContext) : ISyncConfigurationRepository
{
    public async Task<IReadOnlyList<SyncRoot>> GetRootsAsync(
        string accountEmail,
        CancellationToken cancellationToken = default) =>
        await dbContext.SyncRoots
            .Where(r => r.AccountEmail == accountEmail)
            .OrderBy(r => r.LocalPath)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<FolderSelection>> GetFolderSelectionsAsync(
        string accountEmail,
        CancellationToken cancellationToken = default) =>
        await dbContext.FolderSelections
            .Where(f => f.AccountEmail == accountEmail)
            .OrderBy(f => f.FolderPath)
            .ToListAsync(cancellationToken);

    public async Task AddRootAsync(SyncRoot root, CancellationToken cancellationToken = default)
    {
        await dbContext.SyncRoots.AddAsync(root, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveRootAsync(
        string accountEmail,
        string localPath,
        CancellationToken cancellationToken = default)
    {
        var root = await dbContext.SyncRoots
            .FirstOrDefaultAsync(
                r => r.AccountEmail == accountEmail && r.LocalPath == localPath,
                cancellationToken);

        if (root != null)
        {
            dbContext.SyncRoots.Remove(root);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateFolderSelectionsAsync(
        string accountEmail,
        IEnumerable<FolderSelection> selections,
        CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.FolderSelections
            .Where(f => f.AccountEmail == accountEmail)
            .ToListAsync(cancellationToken);

        dbContext.FolderSelections.RemoveRange(existing);
        await dbContext.FolderSelections.AddRangeAsync(selections, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<FolderSelection?> GetFolderSelectionAsync(
        string accountEmail,
        string folderId,
        CancellationToken cancellationToken = default) =>
        await dbContext.FolderSelections
            .FirstOrDefaultAsync(
                f => f.AccountEmail == accountEmail && f.FolderId == folderId,
                cancellationToken);
}
