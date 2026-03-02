namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;

using Accounts;
using Accounts.Models;
using Models;

/// <summary>
/// Scheduler responsible for cadence enforcement and startup auto-sync queueing.
/// </summary>
public class SyncScheduler
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISyncOrchestrator _orchestrator;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SyncScheduler"/> class.
    /// </summary>
    /// <param name="accountRepository">Repository for account access.</param>
    /// <param name="orchestrator">Orchestrator for queuing accounts.</param>
    /// <param name="timeProvider">Time provider for cadence calculations.</param>
    public SyncScheduler(
        IAccountRepository accountRepository,
        ISyncOrchestrator orchestrator,
        TimeProvider timeProvider)
    {
        _accountRepository = accountRepository;
        _orchestrator = orchestrator;
        _timeProvider = timeProvider;
    }

    /// <summary>
    /// Starts the scheduler and enqueues all auto-sync-enabled accounts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether an account is eligible for sync based on cadence rules and current state.
    /// </summary>
    /// <param name="accountId">The account to check.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Eligibility result with reason if not eligible.</returns>
    public Task<EligibilityResult> CanSyncAccountAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
