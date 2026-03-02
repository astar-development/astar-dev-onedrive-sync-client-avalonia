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
    public Task<DeltaResponse> GetInitialDeltaAsync(string accountEmail, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<DeltaResponse> GetDeltaChangesAsync(string accountEmail, string deltaToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
