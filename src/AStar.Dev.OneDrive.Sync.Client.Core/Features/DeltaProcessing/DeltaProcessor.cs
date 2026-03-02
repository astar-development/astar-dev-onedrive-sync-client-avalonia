namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using Accounts.Models;
using Models;

/// <summary>
/// Orchestrates delta processing including discovery, incremental sync, and deduplication.
/// </summary>
public class DeltaProcessor(IGraphDeltaClient graphClient, IDriveItemRepository driveItemRepository) : IDeltaProcessor
{
    private readonly IGraphDeltaClient _graphClient = graphClient;
    private readonly IDriveItemRepository _driveItemRepository = driveItemRepository;

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
