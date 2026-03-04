namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;

/// <summary>
/// Enumerates reasons why an account cannot be synced at a given time.
/// </summary>
public enum IneligibilityReason
{
    /// <summary>
    /// The account does not exist in the system.
    /// </summary>
    AccountNotFound,

    /// <summary>
    /// The cadence window has not elapsed since the last sync completion.
    /// Primary accounts require 15 minutes; secondary accounts require 1 hour.
    /// </summary>
    CadenceWindowNotMet,

    /// <summary>
    /// The system is offline and cannot connect to Microsoft Graph.
    /// </summary>
    Offline,

    /// <summary>
    /// The account is already queued or executing.
    /// </summary>
    AlreadyQueued
}
