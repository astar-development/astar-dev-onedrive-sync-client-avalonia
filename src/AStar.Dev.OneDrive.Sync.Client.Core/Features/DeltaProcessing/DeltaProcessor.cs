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
    public async Task ProcessInitialDiscoveryAsync(SyncAccount account, CancellationToken cancellationToken = default)
    {
        var response = await _graphClient.GetInitialDeltaAsync(account.Email, cancellationToken);
        
        var token = ExtractTokenFromDeltaLink(response.DeltaLink);
        await _driveItemRepository.SaveDeltaTokenAsync(account.Email, token, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task ProcessIncrementalSyncAsync(SyncAccount account, CancellationToken cancellationToken = default)
    {
        var currentToken = await _driveItemRepository.GetDeltaTokenAsync(account.Email, cancellationToken);
        if (currentToken == null)
        {
            await ProcessInitialDiscoveryAsync(account, cancellationToken);
            return;
        }

        var response = await _graphClient.GetDeltaChangesAsync(account.Email, currentToken, cancellationToken);

        var deduplicated = DeduplicateItems(response.Items);

        await PersistInBatchesAsync(account.Email, deduplicated, cancellationToken);

        var newToken = ExtractTokenFromDeltaLink(response.DeltaLink);
        await _driveItemRepository.SaveDeltaTokenAsync(account.Email, newToken, cancellationToken);
    }

    private static IReadOnlyList<DriveItemDto> DeduplicateItems(IReadOnlyList<DriveItemDto> items)
    {
        var grouped = items
            .GroupBy(item => item.Id)
            .Select(group => group.Count() == 1
                ? group.First()
                : group.OrderByDescending(item => item.LastModifiedUtc)
                       .ThenBy(item => item.Id)
                       .First())
            .ToList();

        return grouped;
    }

    private async Task PersistInBatchesAsync(string accountEmail, IReadOnlyList<DriveItemDto> items, CancellationToken cancellationToken)
    {
        const int batchSize = 50;
        
        for (var i = 0; i < items.Count; i += batchSize)
        {
            var batch = items.Skip(i).Take(batchSize).ToList();
            await _driveItemRepository.SaveBatchAsync(accountEmail, batch, cancellationToken);
        }
    }

    private static string ExtractTokenFromDeltaLink(string deltaLink)
    {
        var uri = new Uri(deltaLink);
        var query = uri.Query;
        var tokenParam = query.Split('&')
            .FirstOrDefault(p => p.Contains("token=", StringComparison.OrdinalIgnoreCase));

        if (tokenParam == null)
        {
            throw new InvalidOperationException($"Delta link does not contain a token: {deltaLink}");
        }

        return tokenParam.Split('=')[1];
    }
}
