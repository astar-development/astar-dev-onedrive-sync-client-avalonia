namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;

/// <summary>
/// Represents the outcome of a sync execution.
/// </summary>
public abstract record SyncResult
{
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

    private record SuccessResult() : SyncResult;

    private record FailureResult(string ErrorMessage) : SyncResult;
}
