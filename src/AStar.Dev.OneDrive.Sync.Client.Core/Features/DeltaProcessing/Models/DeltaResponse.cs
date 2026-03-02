namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing.Models;

/// <summary>
/// Represents a delta response from Microsoft Graph containing changed items and a continuation token.
/// </summary>
public record DeltaResponse
{
    /// <summary>
    /// Gets the collection of changed drive items.
    /// </summary>
    public IReadOnlyList<DriveItemDto> Items { get; init; } = [];

    /// <summary>
    /// Gets the delta link containing the next delta token for incremental sync.
    /// Format: https://graph.microsoft.com/v1.0/delta?token={token}
    /// </summary>
    public string DeltaLink { get; init; } = string.Empty;
}
