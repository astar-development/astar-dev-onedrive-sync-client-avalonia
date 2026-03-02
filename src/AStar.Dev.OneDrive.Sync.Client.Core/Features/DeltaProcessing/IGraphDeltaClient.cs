namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Models;

/// <summary>
/// Graph delta client with built-in retry logic for rate limiting.
/// </summary>
public interface IGraphDeltaClient
{
    /// <summary>
    /// Gets the initial delta response for an account with retry logic.
    /// </summary>
    /// <param name="accountEmail">The account email address.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The initial delta response.</returns>
    Task<DeltaResponse> GetInitialDeltaAsync(string accountEmail, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets incremental delta changes for an account with retry logic.
    /// </summary>
    /// <param name="accountEmail">The account email address.</param>
    /// <param name="deltaToken">The delta token from the previous sync.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The delta response with changes.</returns>
    Task<DeltaResponse> GetDeltaChangesAsync(string accountEmail, string deltaToken, CancellationToken cancellationToken = default);
}
