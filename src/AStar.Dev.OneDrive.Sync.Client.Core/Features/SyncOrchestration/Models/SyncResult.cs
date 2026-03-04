namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;

/// <summary>
/// Represents the outcome of a sync execution.
/// </summary>
public abstract record SyncResult
{
    /// <summary>
    /// Gets a value indicating whether the sync was successful.
    /// </summary>
    public abstract bool IsSuccess { get; }

    /// <summary>
    /// Gets the error message if the sync failed; otherwise null.
    /// </summary>
    public abstract string? ErrorMessage { get; }

    /// <summary>
    /// Indicates a successful sync execution.
    /// </summary>
    public static SyncResult Success
        => new SuccessResult();

    /// <summary>
    /// Creates a failed sync result with an error message.
    /// </summary>
    /// <param name="errorMessage">The error description.</param>
    public static SyncResult Failed(string errorMessage)
        => new FailureResult(errorMessage);

    private sealed record SuccessResult() : SyncResult
    {
        public override bool IsSuccess
            => true;

        public override string? ErrorMessage
            => null;
    }

    private sealed record FailureResult(string Error) : SyncResult
    {
        public override bool IsSuccess
            => false;

        public override string? ErrorMessage
            => Error;
    }
}
