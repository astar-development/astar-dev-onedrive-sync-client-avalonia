namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration;

using Models;

/// <summary>
/// Repository contract for sync configuration query and command operations.
/// </summary>
public interface ISyncConfigurationRepository
{
    /// <summary>
    /// Retrieves all sync roots for an account.
    /// </summary>
    /// <param name="accountEmail">The email address of the account.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Collection of sync roots for the account; empty if none configured.</returns>
    Task<IReadOnlyList<SyncRoot>> GetRootsAsync(string accountEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all folder selections (include/exclude state) for an account.
    /// </summary>
    /// <param name="accountEmail">The email address of the account.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Collection of folder selections for the account; empty if none exist.</returns>
    Task<IReadOnlyList<FolderSelection>> GetFolderSelectionsAsync(string accountEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new sync root to the database.
    /// </summary>
    /// <param name="root">The sync root to add; must have non-empty account email and local path.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task AddRootAsync(SyncRoot root, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a sync root by email and local filesystem path.
    /// </summary>
    /// <param name="accountEmail">The email address of the account.</param>
    /// <param name="localPath">The absolute local filesystem path of the root to remove.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task RemoveRootAsync(string accountEmail, string localPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atomically replaces all folder selections for an account.
    /// All existing selections for the account are removed; new selections are inserted.
    /// </summary>
    /// <param name="accountEmail">The email address of the account.</param>
    /// <param name="selections">The complete set of folder selections to persist for this account.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task UpdateFolderSelectionsAsync(string accountEmail, IEnumerable<FolderSelection> selections, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single folder selection by email and folder ID.
    /// </summary>
    /// <param name="accountEmail">The email address of the account.</param>
    /// <param name="folderId">The folder ID from Microsoft Graph.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The folder selection if found; null if not found.</returns>
    Task<FolderSelection?> GetFolderSelectionAsync(string accountEmail, string folderId, CancellationToken cancellationToken = default);
}
