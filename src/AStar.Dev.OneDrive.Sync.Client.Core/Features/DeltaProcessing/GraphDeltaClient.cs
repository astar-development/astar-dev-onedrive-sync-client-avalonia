namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Models;

/// <summary>
/// Graph delta client with built-in retry logic for rate limiting and exponential backoff.
/// </summary>
public class GraphDeltaClient : IGraphDeltaClient
{
    private readonly IGraphApiAdapter _graphAdapter;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDeltaClient"/> class.
    /// </summary>
    /// <param name="graphAdapter">Low-level Graph API adapter.</param>
    /// <param name="timeProvider">Time provider for delay calculations.</param>
    public GraphDeltaClient(
        IGraphApiAdapter graphAdapter,
        TimeProvider timeProvider)
    {
        _graphAdapter = graphAdapter;
        _timeProvider = timeProvider;
    }

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
