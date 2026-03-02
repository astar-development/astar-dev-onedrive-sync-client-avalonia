namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;

using Accounts.Models;
using Models;

/// <summary>
/// Manages the serial execution queue and account sync state transitions.
/// </summary>
public interface ISyncOrchestrator
{
    /// <summary>
    /// Adds an account to the sync queue if not already queued or executing.
    /// </summary>
    /// <param name="accountId">The account to enqueue.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task EnqueueAccountAsync(AccountId accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes the queue serially, executing one account at a time until the queue is empty or cancellation is requested.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task ProcessQueueAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the current sync status of an account for UI observation.
    /// </summary>
    /// <param name="accountId">The account identifier.</param>
    /// <returns>The current sync status; Idle if the account is not tracked.</returns>
    AccountSyncStatus GetAccountStatus(AccountId accountId);
}
