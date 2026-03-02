namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts.Models;

/// <summary>
/// Persistent entity representing a OneDrive account configured for sync.
/// </summary>
/// <remarks>
/// Must be a class (not record) for EF Core tracking.
/// Natural key is Email; IsPrimary constraint enforces at most one primary per system.
/// </remarks>
public class SyncAccount
{
    /// <summary>
    /// Gets or inits the email address, serving as the natural identifier.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this is the designated primary account for multi-account contexts.
    /// Invariant: exactly one account must have IsPrimary=true when any accounts exist.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Gets or sets the UTC time when this account was added to the system.
    /// </summary>
    public DateTimeOffset AddedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC time of the last successful account sync, if any.
    /// </summary>
    public DateTimeOffset? LastSyncAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the encrypted OAuth refresh token for token renewal.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the encrypted OAuth access token for Graph API calls.
    /// </summary>
    /// <remarks>
    /// Access tokens are short-lived; automatically renewed via refresh token during sync.
    /// </remarks>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the UTC expiration time of the access token.
    /// </summary>
    public DateTimeOffset? AccessTokenExpiresAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the user's display name retrieved from Microsoft Graph.
    /// </summary>
    public string? DisplayName { get; set; }
}
