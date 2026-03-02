namespace AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts;

using Models;

/// <summary>
/// Repository contract for account query and command operations.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Retrieves all sync accounts ordered by email.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>Collection of all configured accounts; empty if none exist.</returns>
    Task<IReadOnlyList<SyncAccount>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an account by its natural key (email address).
    /// </summary>
    /// <param name="accountId">The account identifier (email).</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The account if found; null if not found.</returns>
    Task<SyncAccount?> GetByEmailAsync(AccountId accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the designated primary account, if one exists.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    /// <returns>The primary account if one is designated; null otherwise.</returns>
    Task<SyncAccount?> GetPrimaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new account to the database.
    /// </summary>
    /// <param name="account">The account to add; must have a non-empty email address.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task AddAsync(SyncAccount account, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes an account and its associated configuration from the database.
    /// </summary>
    /// <param name="accountId">The account identifier (email) of the account to remove.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task RemoveAsync(AccountId accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing account's properties in the database.
    /// </summary>
    /// <param name="account">The account with updated properties; identified by its email address.</param>
    /// <param name="cancellationToken">Cancellation token for the asynchronous operation.</param>
    Task UpdateAsync(SyncAccount account, CancellationToken cancellationToken = default);
}
