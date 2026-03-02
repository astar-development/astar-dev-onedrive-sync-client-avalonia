namespace AStar.Dev.OneDrive.Sync.Client.Core.Foundation;

/// <summary>
/// Immutable base type for domain events representing significant business occurrences.
/// </summary>
public abstract record DomainEvent
{
    /// <summary>
    /// Gets the UTC time when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAtUtc { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets a unique identifier for this event instance for deduplication and audit trails.
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();
}
