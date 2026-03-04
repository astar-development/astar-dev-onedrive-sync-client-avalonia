namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;

using System.Web;
using Accounts.Models;
using Models;

/// <summary>
/// Orchestrates delta processing including discovery, incremental sync, and deduplication.
/// </summary>
public class DeltaProcessor(IGraphDeltaClient graphClient, IDriveItemRepository driveItemRepository) : IDeltaProcessor
{
    private const int BatchSize = 50;
    
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

        var deduplicated = DeduplicateWithLastModifiedWins(response.Items);

        await PersistInBatchesAsync(account.Email, deduplicated, cancellationToken);

        var newToken = ExtractTokenFromDeltaLink(response.DeltaLink);
        await _driveItemRepository.SaveDeltaTokenAsync(account.Email, newToken, cancellationToken);
    }

    private static IReadOnlyList<DriveItemDto> DeduplicateWithLastModifiedWins(IReadOnlyList<DriveItemDto> items)
        => items
            .GroupBy(item => item.Id)
            .Select(SelectMostRecentItem)
            .ToList();

    private static DriveItemDto SelectMostRecentItem(IGrouping<string, DriveItemDto> group)
        => group
            .OrderByDescending(item => item.LastModifiedUtc)
            .ThenBy(item => item.Id)
            .First();

    private async Task PersistInBatchesAsync(string accountEmail, IReadOnlyList<DriveItemDto> items, CancellationToken cancellationToken)
    {
        for (var i = 0; i < items.Count; i += BatchSize)
        {
            var batch = items.Skip(i).Take(BatchSize).ToList();
            await _driveItemRepository.SaveBatchAsync(accountEmail, batch, cancellationToken);
        }
    }

    private static string ExtractTokenFromDeltaLink(string deltaLink)
    {
        var uri = new Uri(deltaLink);
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        var token = queryParams["token"];

        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException($"Delta link does not contain a token: {deltaLink}");
        }

        return token;
    }
}
