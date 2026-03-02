namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;

using Accounts;
using Accounts.Models;
using Models;

/// <summary>
/// Orchestrates serial execution of account sync operations and manages sync state for UI observation.
/// </summary>
public class SyncOrchestrator : ISyncOrchestrator
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISyncExecutor _syncExecutor;

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncOrchestrator"/> class.
    /// </summary>
    /// <param name="accountRepository">Repository for account access.</param>
    /// <param name="syncExecutor">Executor for performing sync operations.</param>
    public SyncOrchestrator(
        IAccountRepository accountRepository,
        ISyncExecutor syncExecutor)
    {
        _accountRepository = accountRepository;
        _syncExecutor = syncExecutor;
    }

    /// <inheritdoc/>
    public Task EnqueueAccountAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task ProcessQueueAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public AccountSyncStatus GetAccountStatus(AccountId accountId)
    {
        throw new NotImplementedException();
    }
}
