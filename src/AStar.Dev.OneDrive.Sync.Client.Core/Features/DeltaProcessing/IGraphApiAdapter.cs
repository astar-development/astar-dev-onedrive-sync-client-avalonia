namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Models;

/// <summary>
/// Low-level adapter for Microsoft Graph delta API calls.
/// </summary>
public interface IGraphApiAdapter
{
    /// <summary>
    /// Calls the Microsoft Graph delta endpoint for an account.
    /// </summary>
    /// <param name="accountEmail">The account email address.</param>
    /// <param name="deltaToken">The delta token for incremental sync; null for initial discovery.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The delta response containing items and next delta link.</returns>
    /// <exception cref="GraphRateLimitException">Thrown when rate limit (429) is encountered.</exception>
    Task<DeltaResponse> GetDeltaAsync(string accountEmail, string? deltaToken, CancellationToken cancellationToken = default);
}
