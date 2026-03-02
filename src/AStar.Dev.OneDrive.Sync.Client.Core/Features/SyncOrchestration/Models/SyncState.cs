namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;

/// <summary>
/// Represents the current synchronization state of an account.
/// </summary>
public enum SyncState
{
    /// <summary>
    /// The account is not currently syncing and has no pending operations.
    /// </summary>
    Idle,

    /// <summary>
    /// The account is queued for synchronization but not yet executing.
    /// </summary>
    Queued,

    /// <summary>
    /// The account is actively being synchronized.
    /// </summary>
    Syncing,

    /// <summary>
    /// The last sync operation completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// The last sync operation failed.
    /// </summary>
    Failed
}
