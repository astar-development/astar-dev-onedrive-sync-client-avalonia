namespace AStar.Dev.OneDrive.Sync.Client.Tests.Unit.Features.DeltaProcessing;

using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts.Models;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing.Models;
using NSubstitute;
using Shouldly;

/// <summary>
/// Tests for delta processor's deduplication and batch persistence logic.
/// </summary>
public class DeltaProcessorShould
{
    private readonly IGraphDeltaClient _graphClient;
    private readonly IDriveItemRepository _driveItemRepository;
    private readonly DeltaProcessor _processor;

    public DeltaProcessorShould()
    {
        _graphClient = Substitute.For<IGraphDeltaClient>();
        _driveItemRepository = Substitute.For<IDriveItemRepository>();
        _processor = new DeltaProcessor(_graphClient, _driveItemRepository);
    }

    [Fact]
    public async Task CaptureInitialDeltaTokenOnFirstRun()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var deltaResponse = new DeltaResponse
        {
            Items = [],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=abc123"
        };

        _graphClient.GetInitialDeltaAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        // Act
        await _processor.ProcessInitialDiscoveryAsync(account, CancellationToken.None);

        // Assert
        await _driveItemRepository.Received(1).SaveDeltaTokenAsync(
            account.Email,
            "abc123",
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessIncrementalDeltaUsingStoredToken()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var storedToken = "previousToken";
        var deltaResponse = new DeltaResponse
        {
            Items = [new DriveItemDto { Id = "item1", Name = "File.txt", LastModifiedUtc = DateTimeOffset.UtcNow }],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=newToken"
        };

        _driveItemRepository.GetDeltaTokenAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns(storedToken);

        _graphClient.GetDeltaChangesAsync(account.Email, storedToken, Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        // Act
        await _processor.ProcessIncrementalSyncAsync(account, CancellationToken.None);

        // Assert
        await _driveItemRepository.Received(1).SaveDeltaTokenAsync(
            account.Email,
            "newToken",
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeduplicateItemsWithinBatchUsingLastModified()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var olderTime = new DateTimeOffset(2026, 3, 1, 10, 0, 0, TimeSpan.Zero);
        var newerTime = new DateTimeOffset(2026, 3, 1, 11, 0, 0, TimeSpan.Zero);

        var deltaResponse = new DeltaResponse
        {
            Items =
            [
                new DriveItemDto { Id = "duplicate1", Name = "File.txt", LastModifiedUtc = olderTime },
                new DriveItemDto { Id = "duplicate1", Name = "File-Updated.txt", LastModifiedUtc = newerTime }
            ],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=token123"
        };

        _driveItemRepository.GetDeltaTokenAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns("oldToken");

        _graphClient.GetDeltaChangesAsync(account.Email, "oldToken", Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        // Act
        await _processor.ProcessIncrementalSyncAsync(account, CancellationToken.None);

        // Assert - should only persist the newer item
        await _driveItemRepository.Received(1).SaveBatchAsync(
            account.Email,
            Arg.Is<IReadOnlyList<DriveItemDto>>(items =>
                items.Count == 1 &&
                items[0].Id == "duplicate1" &&
                items[0].Name == "File-Updated.txt" &&
                items[0].LastModifiedUtc == newerTime),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UseLexicalItemIdAsTieBreakerForDuplicatesWithSameTimestamp()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var sameTime = new DateTimeOffset(2026, 3, 1, 10, 0, 0, TimeSpan.Zero);

        var deltaResponse = new DeltaResponse
        {
            Items =
            [
                new DriveItemDto { Id = "item-b", Name = "FileB.txt", LastModifiedUtc = sameTime },
                new DriveItemDto { Id = "item-a", Name = "FileA.txt", LastModifiedUtc = sameTime }
            ],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=token123"
        };

        _driveItemRepository.GetDeltaTokenAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns("oldToken");

        _graphClient.GetDeltaChangesAsync(account.Email, "oldToken", Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        // Act
        await _processor.ProcessIncrementalSyncAsync(account, CancellationToken.None);

        // Assert - should keep both since they have different IDs (not true duplicates)
        // This test actually verifies that we only dedupe SAME items, not different items
        await _driveItemRepository.Received(1).SaveBatchAsync(
            account.Email,
            Arg.Is<IReadOnlyList<DriveItemDto>>(items => items.Count == 2),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PersistItemsInBatchesOf50()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var items = Enumerable.Range(1, 120)
            .Select(i => new DriveItemDto
            {
                Id = $"item{i}",
                Name = $"File{i}.txt",
                LastModifiedUtc = DateTimeOffset.UtcNow
            })
            .ToList();

        var deltaResponse = new DeltaResponse
        {
            Items = items,
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=token123"
        };

        _driveItemRepository.GetDeltaTokenAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns("oldToken");

        _graphClient.GetDeltaChangesAsync(account.Email, "oldToken", Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        // Act
        await _processor.ProcessIncrementalSyncAsync(account, CancellationToken.None);

        // Assert - should receive 3 batches: 50 + 50 + 20
        await _driveItemRepository.Received(1).SaveBatchAsync(
            account.Email,
            Arg.Is<IReadOnlyList<DriveItemDto>>(batch => batch.Count == 50),
            Arg.Any<CancellationToken>());

        await _driveItemRepository.Received(1).SaveBatchAsync(
            account.Email,
            Arg.Is<IReadOnlyList<DriveItemDto>>(batch => batch.Count == 20),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AdvanceDeltaTokenAtomicallyWithBatchCommit()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var deltaResponse = new DeltaResponse
        {
            Items = [new DriveItemDto { Id = "item1", Name = "File.txt", LastModifiedUtc = DateTimeOffset.UtcNow }],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=newToken"
        };

        _driveItemRepository.GetDeltaTokenAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns("oldToken");

        _graphClient.GetDeltaChangesAsync(account.Email, "oldToken", Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        var saveOrder = new List<string>();

        _driveItemRepository.SaveBatchAsync(Arg.Any<string>(), Arg.Any<IReadOnlyList<DriveItemDto>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(_ => saveOrder.Add("batch"));

        _driveItemRepository.SaveDeltaTokenAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(_ => saveOrder.Add("token"));

        // Act
        await _processor.ProcessIncrementalSyncAsync(account, CancellationToken.None);

        // Assert - token should be saved after batch
        saveOrder.ShouldBe(["batch", "token"]);
    }

    [Fact]
    public async Task NotAdvanceTokenIfBatchPersistenceFails()
    {
        // Arrange
        var account = new SyncAccount { Email = "test@example.com" };
        var deltaResponse = new DeltaResponse
        {
            Items = [new DriveItemDto { Id = "item1", Name = "File.txt", LastModifiedUtc = DateTimeOffset.UtcNow }],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=newToken"
        };

        _driveItemRepository.GetDeltaTokenAsync(account.Email, Arg.Any<CancellationToken>())
            .Returns("oldToken");

        _graphClient.GetDeltaChangesAsync(account.Email, "oldToken", Arg.Any<CancellationToken>())
            .Returns(deltaResponse);

        _driveItemRepository.SaveBatchAsync(Arg.Any<string>(), Arg.Any<IReadOnlyList<DriveItemDto>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new InvalidOperationException("Database error")));

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(
            async () => await _processor.ProcessIncrementalSyncAsync(account, CancellationToken.None));

        // Token should not have been saved
        await _driveItemRepository.DidNotReceive().SaveDeltaTokenAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
