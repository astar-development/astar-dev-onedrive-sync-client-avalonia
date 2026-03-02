namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Accounts.Models;
using Models;

/// <summary>
/// Orchestrates delta processing including discovery, incremental sync, and deduplication.
/// </summary>
public interface IDeltaProcessor
{
    /// <summary>
    /// Performs initial discovery for an account and captures the baseline delta token.
    /// </summary>
    /// <param name="account">The account to discover.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task ProcessInitialDiscoveryAsync(SyncAccount account, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes incremental changes for an account using the stored delta token.
    /// Deduplicates items within each batch and persists in batches of 50.
    /// </summary>
    /// <param name="account">The account to sync.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task ProcessIncrementalSyncAsync(SyncAccount account, CancellationToken cancellationToken = default);
}
