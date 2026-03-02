namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Accounts.Models;
using Models;

/// <summary>
/// Orchestrates delta processing including discovery, incremental sync, and deduplication.
/// </summary>
public class DeltaProcessor : IDeltaProcessor
{
    private readonly IGraphDeltaClient _graphClient;
    private readonly IDriveItemRepository _driveItemRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeltaProcessor"/> class.
    /// </summary>
    /// <param name="graphClient">Client for delta API calls with retry logic.</param>
    /// <param name="driveItemRepository">Repository for persisting items and tokens.</param>
    public DeltaProcessor(
        IGraphDeltaClient graphClient,
        IDriveItemRepository driveItemRepository)
    {
        _graphClient = graphClient;
        _driveItemRepository = driveItemRepository;
    }

    /// <inheritdoc/>
    public Task ProcessInitialDiscoveryAsync(SyncAccount account, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task ProcessIncrementalSyncAsync(SyncAccount account, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
