namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;

/// <summary>
/// Represents the eligibility status of an account for sync execution.
/// </summary>
/// <param name="IsEligible">Indicates whether the account can be synced at this time.</param>
/// <param name="Reason">The reason for ineligibility; null if eligible.</param>
public record EligibilityResult(bool IsEligible, IneligibilityReason? Reason = null);
