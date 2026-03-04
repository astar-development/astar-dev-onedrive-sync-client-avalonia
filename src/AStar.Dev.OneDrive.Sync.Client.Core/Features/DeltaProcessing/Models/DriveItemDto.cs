namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing.Models;

/// <summary>
/// Data transfer object representing a OneDrive drive item from Microsoft Graph.
/// </summary>
public record DriveItemDto
{
    /// <summary>
    /// Gets the unique identifier of the drive item from Microsoft Graph.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Gets the name of the file or folder.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the UTC timestamp when the item was last modified.
    /// Used for deduplication and conflict resolution.
    /// </summary>
    public DateTimeOffset LastModifiedUtc { get; init; }

    /// <summary>
    /// Gets a value indicating whether this item is a folder.
    /// </summary>
    public bool IsFolder { get; init; }

    /// <summary>
    /// Gets the parent folder identifier; null for root items.
    /// </summary>
    public string? ParentId { get; init; }

    /// <summary>
    /// Gets the size of the file in bytes; null for folders.
    /// </summary>
    public long? Size { get; init; }

    /// <summary>
    /// Gets a value indicating whether this item was deleted.
    /// </summary>
    public bool IsDeleted { get; init; }
}
