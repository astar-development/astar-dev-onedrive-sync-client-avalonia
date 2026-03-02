namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;

using Accounts.Models;

/// <summary>
/// Represents the observable status of an account's sync operations for UI binding.
/// </summary>
/// <param name="AccountId">The account identifier.</param>
/// <param name="State">The current sync state.</param>
/// <param name="ErrorMessage">Error details if State is Failed; otherwise null.</param>
/// <param name="LastSyncAtUtc">The UTC timestamp of the last completed sync; null if never synced.</param>
public record AccountSyncStatus(
    AccountId AccountId,
    SyncState State,
    string? ErrorMessage = null,
    DateTimeOffset? LastSyncAtUtc = null);
