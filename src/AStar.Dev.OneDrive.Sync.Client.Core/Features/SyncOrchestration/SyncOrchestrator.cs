namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;

using System.Collections.Concurrent;
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
    private readonly ConcurrentQueue<AccountId> _queue = new();
    private readonly ConcurrentDictionary<string, AccountSyncStatus> _statusMap = new();
    private readonly SemaphoreSlim _executionLock = new(1, 1);

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
        // Check if already queued or syncing
        if (_statusMap.TryGetValue(accountId.Email, out var currentStatus))
        {
            if (currentStatus.State == SyncState.Queued || currentStatus.State == SyncState.Syncing)
            {
                return Task.CompletedTask;
            }
        }

        _queue.Enqueue(accountId);
        _statusMap[accountId.Email] = new AccountSyncStatus(accountId, SyncState.Queued);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task ProcessQueueAsync(CancellationToken cancellationToken = default)
    {
        while (_queue.TryDequeue(out var accountId))
        {
            await _executionLock.WaitAsync(cancellationToken);
            try
            {
                var account = await _accountRepository.GetByEmailAsync(accountId, cancellationToken);
                if (account == null)
                {
                    _statusMap[accountId.Email] = new AccountSyncStatus(
                        accountId, 
                        SyncState.Failed, 
                        "Account not found");
                    continue;
                }

                _statusMap[accountId.Email] = new AccountSyncStatus(accountId, SyncState.Syncing);

                var result = await _syncExecutor.ExecuteSyncAsync(account, cancellationToken);

                _statusMap[accountId.Email] = result switch
                {
                    { } success when IsSuccess(result)
                        => new AccountSyncStatus(accountId, SyncState.Idle),
                    _ => new AccountSyncStatus(
                        accountId, 
                        SyncState.Failed, 
                        GetErrorMessage(result))
                };
            }
            finally
            {
                _executionLock.Release();
            }
        }
    }

    /// <inheritdoc/>
    public AccountSyncStatus GetAccountStatus(AccountId accountId)
        => _statusMap.TryGetValue(accountId.Email, out var status)
            ? status
            : new AccountSyncStatus(accountId, SyncState.Idle);

    private static bool IsSuccess(SyncResult result)
        => result == SyncResult.Success;

    private static string GetErrorMessage(SyncResult result)
        => result switch
        {
            _ when result.GetType().GetProperty("ErrorMessage") is { } prop 
                => prop.GetValue(result) as string ?? "Unknown error",
            _ => "Unknown error"
        };
}
