namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Models;

/// <summary>
/// Graph delta client with built-in retry logic for rate limiting and exponential backoff.
/// </summary>
public class GraphDeltaClient(IGraphApiAdapter graphAdapter, TimeProvider timeProvider) : IGraphDeltaClient
{
    private readonly IGraphApiAdapter _graphAdapter = graphAdapter;
    private readonly TimeProvider _timeProvider = timeProvider;

    /// <inheritdoc/>
    public async Task<DeltaResponse> GetInitialDeltaAsync(string accountEmail, CancellationToken cancellationToken = default)
        => await ExecuteWithRetryAsync(() => _graphAdapter.GetDeltaAsync(accountEmail, null, cancellationToken), cancellationToken);

    /// <inheritdoc/>
    public async Task<DeltaResponse> GetDeltaChangesAsync(string accountEmail, string deltaToken, CancellationToken cancellationToken = default)
        => await ExecuteWithRetryAsync(() => _graphAdapter.GetDeltaAsync(accountEmail, deltaToken, cancellationToken), cancellationToken);

    private async Task<DeltaResponse> ExecuteWithRetryAsync(Func<Task<DeltaResponse>> operation, CancellationToken cancellationToken)
    {
        const int maxRetries = 5;
        var attemptCount = 0;

        while (true)
        {
            try
            {
                return await operation();
            }
            catch (GraphRateLimitException ex) when (attemptCount < maxRetries)
            {
                attemptCount++;
                
                if (attemptCount >= maxRetries)
                {
                    throw;
                }

                var delay = ex.RetryAfter ?? TimeSpan.FromSeconds(Math.Pow(2, attemptCount));
                
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
