namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration.Models;

/// <summary>
/// Persistent entity representing a local filesystem directory designated for OneDrive sync.
/// </summary>
/// <remarks>
/// Must be a class (not record) for EF Core tracking.
/// Natural key is the composite (AccountEmail, LocalPath); prevents duplicate roots per account.
/// </remarks>
public class SyncRoot
{
    /// <summary>
    /// Gets or inits the account email address; part of composite natural key.
    /// </summary>
    public string AccountEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets or inits the absolute local filesystem path; part of composite natural key.
    /// </summary>
    public string LocalPath { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC time when this root was configured.
    /// </summary>
    public DateTimeOffset ConfiguredAtUtc { get; set; }

    /// <summary>
    /// Gets or sets whether this root is currently active (enabled for sync).
    /// </summary>
    public bool IsActive { get; set; } = true;
}
