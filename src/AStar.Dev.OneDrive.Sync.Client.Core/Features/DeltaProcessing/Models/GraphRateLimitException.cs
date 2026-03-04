namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing.Models;

/// <summary>
/// Exception thrown when Microsoft Graph API returns 429 Too Many Requests.
/// </summary>
public class GraphRateLimitException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphRateLimitException"/> class.
    /// </summary>
    /// <param name="retryAfter">The duration to wait before retrying; null if not specified by server.</param>
    public GraphRateLimitException(TimeSpan? retryAfter)
        : base($"Rate limit exceeded. Retry after: {retryAfter?.TotalSeconds ?? 0} seconds")
    {
        RetryAfter = retryAfter;
    }

    /// <summary>
    /// Gets the duration to wait before retrying, if specified by the server.
    /// </summary>
    public TimeSpan? RetryAfter { get; }
}
