namespace AStar.Dev.OneDrive.Sync.Client.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts.Models;
using Data;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class AccountRepository(SyncDbContext dbContext) : IAccountRepository
{
    public async Task<IReadOnlyList<SyncAccount>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Accounts
            .OrderBy(a => a.Email)
            .ToListAsync(cancellationToken);

    public async Task<SyncAccount?> GetByEmailAsync(AccountId accountId, CancellationToken cancellationToken = default) =>
        await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Email == accountId.Email, cancellationToken);

    public async Task<SyncAccount?> GetPrimaryAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.IsPrimary, cancellationToken);

    public async Task AddAsync(SyncAccount account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        var account = await dbContext.Accounts.FindAsync(
            [accountId.Email],
            cancellationToken: cancellationToken);

        if (account != null)
        {
            dbContext.Accounts.Remove(account);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateAsync(SyncAccount account, CancellationToken cancellationToken = default)
    {
        dbContext.Accounts.Update(account);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
