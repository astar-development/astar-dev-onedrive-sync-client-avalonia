namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;

using Accounts.Models;
using Models;

/// <summary>
/// Executes the sync operation for a single account, coordinating delta processing and conflict resolution.
/// </summary>
public interface ISyncExecutor
{
    /// <summary>
    /// Executes a full sync cycle for the specified account.
    /// </summary>
    /// <param name="account">The account to sync.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The result of the sync operation.</returns>
    Task<SyncResult> ExecuteSyncAsync(SyncAccount account, CancellationToken cancellationToken = default);
}
