namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;

using Accounts;
using Accounts.Models;
using Models;

/// <summary>
/// Scheduler responsible for cadence enforcement and startup auto-sync queueing.
/// </summary>
public class SyncScheduler(IAccountRepository accountRepository, ISyncOrchestrator orchestrator, TimeProvider timeProvider)
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ISyncOrchestrator _orchestrator = orchestrator;
    private readonly TimeProvider _timeProvider = timeProvider;

    private static readonly TimeSpan PrimaryCadence = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan SecondaryCadence = TimeSpan.FromHours(1);

    /// <summary>
    /// Starts the scheduler and enqueues all auto-sync-enabled accounts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var accounts = await _accountRepository.GetAllAsync(cancellationToken);
        var autoSyncAccounts = accounts.Where(a => a.AutoSyncEnabled);

        foreach (var account in autoSyncAccounts)
        {
            await _orchestrator.EnqueueAccountAsync(new AccountId(account.Email), cancellationToken);
        }
    }

    /// <summary>
    /// Determines whether an account is eligible for sync based on cadence rules and current state.
    /// </summary>
    /// <param name="accountId">The account to check.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Eligibility result with reason if not eligible.</returns>
    public async Task<EligibilityResult> CanSyncAccountAsync(
        AccountId accountId, 
        CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByEmailAsync(accountId, cancellationToken);
        if (account == null)
        {
            return new EligibilityResult(false, IneligibilityReason.AccountNotFound);
        }

        if (account.LastSyncAtUtc == null)
        {
            return new EligibilityResult(true);
        }

        var now = _timeProvider.GetUtcNow();
        var timeSinceLastSync = now - account.LastSyncAtUtc.Value;
        var requiredCadence = account.IsPrimary ? PrimaryCadence : SecondaryCadence;

        if (timeSinceLastSync < requiredCadence)
        {
            return new EligibilityResult(false, IneligibilityReason.CadenceWindowNotMet);
        }

        return new EligibilityResult(true);
    }
}
