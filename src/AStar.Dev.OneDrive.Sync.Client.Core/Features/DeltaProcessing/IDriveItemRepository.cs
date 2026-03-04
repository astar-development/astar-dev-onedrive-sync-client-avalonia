namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Models;

/// <summary>
/// Repository contract for persisting drive items and delta synchronization tokens.
/// </summary>
public interface IDriveItemRepository
{
    /// <summary>
    /// Retrieves the stored delta token for an account.
    /// </summary>
    /// <param name="accountEmail">The account email address.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The delta token if found; null if this is the first sync.</returns>
    Task<string?> GetDeltaTokenAsync(string accountEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the delta token for an account atomically with batch persistence.
    /// </summary>
    /// <param name="accountEmail">The account email address.</param>
    /// <param name="deltaToken">The delta token extracted from the delta link.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task SaveDeltaTokenAsync(string accountEmail, string deltaToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a batch of drive items for an account.
    /// Implements last-update-wins policy for duplicates.
    /// </summary>
    /// <param name="accountEmail">The account email address.</param>
    /// <param name="items">The batch of items to persist; maximum 50 items.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task SaveBatchAsync(string accountEmail, IReadOnlyList<DriveItemDto> items, CancellationToken cancellationToken = default);
}
