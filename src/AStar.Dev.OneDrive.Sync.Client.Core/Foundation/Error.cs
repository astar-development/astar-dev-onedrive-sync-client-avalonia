namespace AStar.Dev.OneDrive.Sync.Client.Core.Foundation;

/// <summary>
/// Immutable error value object representing an application failure.
/// </summary>
/// <param name="Code">Machine-readable error code (e.g., "NotFound.Account").</param>
/// <param name="Message">Human-readable error description.</param>
public record Error(string Code, string Message)
{
    /// <summary>
    /// Factory for resource-not-found errors.
    /// </summary>
    public static Error NotFound(string resource, string identifier) =>
        new($"NotFound.{resource}", $"{resource} with identifier '{identifier}' not found.");

    /// <summary>
    /// Factory for validation failure errors.
    /// </summary>
    public static Error ValidationFailed(string field, string reason) =>
        new($"Validation.{field}", $"Validation failed for {field}: {reason}");

    /// <summary>
    /// Factory for conflict errors (e.g., duplicate entity or constraint violation).
    /// </summary>
    public static Error Conflict(string resource, string reason) =>
        new($"Conflict.{resource}", $"Conflict for {resource}: {reason}");

    /// <summary>
    /// Factory for generic errors with custom code and message.
    /// </summary>
    public static Error Generic(string code, string message) =>
        new(code, message);
}
