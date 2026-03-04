namespace AStar.Dev.OneDrive.Sync.Client.Tests.Unit.Features.SyncOrchestration;

using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts.Models;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;
using NSubstitute;
using Shouldly;

/// <summary>
/// Tests for the sync orchestrator's serial execution guarantees and queue management.
/// </summary>
public class SyncOrchestratorShould
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISyncExecutor _syncExecutor;
    private readonly SyncOrchestrator _orchestrator;

    public SyncOrchestratorShould()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _syncExecutor = Substitute.For<ISyncExecutor>();
        _orchestrator = new SyncOrchestrator(_accountRepository, _syncExecutor);
    }

    [Fact]
    public async Task ExecuteOnlyOneAccountAtATime()
    {

        var account1 = new SyncAccount { Email = "account1@example.com" };
        var account2 = new SyncAccount { Email = "account2@example.com" };

        _accountRepository.GetByEmailAsync(Arg.Is<AccountId>(id => id.Email == "account1@example.com"), Arg.Any<CancellationToken>())
            .Returns(account1);
        _accountRepository.GetByEmailAsync(Arg.Is<AccountId>(id => id.Email == "account2@example.com"), Arg.Any<CancellationToken>())
            .Returns(account2);

        var firstSyncStarted = new TaskCompletionSource<bool>();
        var firstSyncCanComplete = new TaskCompletionSource<bool>();

        _syncExecutor.ExecuteSyncAsync(account1, Arg.Any<CancellationToken>())
            .Returns(async _ =>
            {
                firstSyncStarted.SetResult(true);
                await firstSyncCanComplete.Task;
                return SyncResult.Success;
            });

        _syncExecutor.ExecuteSyncAsync(account2, Arg.Any<CancellationToken>())
            .Returns(SyncResult.Success);


        await _orchestrator.EnqueueAccountAsync(new AccountId("account1@example.com"), CancellationToken.None);
        await _orchestrator.EnqueueAccountAsync(new AccountId("account2@example.com"), CancellationToken.None);

        var processTask = _orchestrator.ProcessQueueAsync(CancellationToken.None);

        // Wait for first sync to start
        await firstSyncStarted.Task;


        await _syncExecutor.Received(1).ExecuteSyncAsync(account1, Arg.Any<CancellationToken>());
        await _syncExecutor.DidNotReceive().ExecuteSyncAsync(account2, Arg.Any<CancellationToken>());

        // Complete first sync and wait for second
        firstSyncCanComplete.SetResult(true);
        await Task.Delay(100); // Allow time for second sync to execute


        await _syncExecutor.Received(1).ExecuteSyncAsync(account2, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAccountStatusDuringSync()
    {

        var account = new SyncAccount { Email = "test@example.com" };
        _accountRepository.GetByEmailAsync(Arg.Any<AccountId>(), Arg.Any<CancellationToken>())
            .Returns(account);

        _syncExecutor.ExecuteSyncAsync(Arg.Any<SyncAccount>(), Arg.Any<CancellationToken>())
            .Returns(SyncResult.Success);


        await _orchestrator.EnqueueAccountAsync(new AccountId("test@example.com"), CancellationToken.None);
        var status = _orchestrator.GetAccountStatus(new AccountId("test@example.com"));


        status.State.ShouldBe(SyncState.Queued);
    }

    [Fact]
    public async Task MarkAccountAsCompletedAfterSuccessfulSync()
    {

        var account = new SyncAccount { Email = "test@example.com" };
        _accountRepository.GetByEmailAsync(Arg.Any<AccountId>(), Arg.Any<CancellationToken>())
            .Returns(account);

        _syncExecutor.ExecuteSyncAsync(Arg.Any<SyncAccount>(), Arg.Any<CancellationToken>())
            .Returns(SyncResult.Success);


        await _orchestrator.EnqueueAccountAsync(new AccountId("test@example.com"), CancellationToken.None);
        await _orchestrator.ProcessQueueAsync(CancellationToken.None);

        var status = _orchestrator.GetAccountStatus(new AccountId("test@example.com"));


        status.State.ShouldBe(SyncState.Idle);
    }

    [Fact]
    public async Task HandleSyncFailuresGracefully()
    {

        var account = new SyncAccount { Email = "failing@example.com" };
        _accountRepository.GetByEmailAsync(Arg.Any<AccountId>(), Arg.Any<CancellationToken>())
            .Returns(account);

        _syncExecutor.ExecuteSyncAsync(Arg.Any<SyncAccount>(), Arg.Any<CancellationToken>())
            .Returns(SyncResult.Failed("Network error"));


        await _orchestrator.EnqueueAccountAsync(new AccountId("failing@example.com"), CancellationToken.None);
        await _orchestrator.ProcessQueueAsync(CancellationToken.None);

        var status = _orchestrator.GetAccountStatus(new AccountId("failing@example.com"));


        status.State.ShouldBe(SyncState.Failed);
        status.ErrorMessage.ShouldBe("Network error");
    }

    [Fact]
    public void ReportIdleStatusForAccountNotInQueue()
    {

        var status = _orchestrator.GetAccountStatus(new AccountId("unknown@example.com"));


        status.State.ShouldBe(SyncState.Idle);
    }

    [Fact]
    public async Task NotEnqueueSameAccountTwice()
    {

        var account = new SyncAccount { Email = "test@example.com" };
        _accountRepository.GetByEmailAsync(Arg.Any<AccountId>(), Arg.Any<CancellationToken>())
            .Returns(account);


        await _orchestrator.EnqueueAccountAsync(new AccountId("test@example.com"), CancellationToken.None);
        await _orchestrator.EnqueueAccountAsync(new AccountId("test@example.com"), CancellationToken.None);

        var status = _orchestrator.GetAccountStatus(new AccountId("test@example.com"));


        status.State.ShouldBe(SyncState.Queued);
    }
}
