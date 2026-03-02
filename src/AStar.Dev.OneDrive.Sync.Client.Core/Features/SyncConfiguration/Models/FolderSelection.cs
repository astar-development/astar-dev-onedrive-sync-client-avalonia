namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration.Models;

/// <summary>
/// Persistent entity representing the inclusion/exclusion state of a OneDrive folder for sync.
/// </summary>
/// <remarks>
/// Must be a class (not record) for EF Core tracking.
/// Natural key is the composite (AccountEmail, FolderId); prevents duplicate folder selections per account.
/// </remarks>
public class FolderSelection
{
    /// <summary>
    /// Gets or inits the account email address; part of composite natural key.
    /// </summary>
    public string AccountEmail { get; init; } = string.Empty;

    /// <summary>
    /// Gets or inits the folder ID from Microsoft Graph; part of composite natural key.
    /// </summary>
    public string FolderId { get; init; } = string.Empty;

    /// <summary>
    /// Gets or inits the folder's display path (e.g., "/Documents/Projects").
    /// </summary>
    public string FolderPath { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this folder is included (true) or excluded (false) from sync.
    /// </summary>
    public bool IsIncluded { get; set; }

    /// <summary>
    /// Gets or sets the UTC time this selection was last modified.
    /// </summary>
    public DateTimeOffset ModifiedAtUtc { get; set; }
}
