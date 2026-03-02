namespace AStar.Dev.OneDrive.Sync.Client.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncConfiguration;
using Data;
using Repositories;

/// <summary>
/// Dependency injection configuration for infrastructure layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers infrastructure services including database context and repositories.
    /// </summary>
    /// <param name="services">The service collection to extend.</param>
    /// <param name="databasePath">Path to SQLite database file; defaults to "sync.db" in current directory.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databasePath = "sync.db") =>
        services
            .AddDbContext<SyncDbContext>(options =>
                options.UseSqlite($"Data Source={databasePath};Version=3;"))
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<ISyncConfigurationRepository, SyncConfigurationRepository>();

    /// <summary>
    /// Initializes the database by applying pending migrations.
    /// Must be called once at application startup.
    /// </summary>
    /// <param name="serviceProvider">The service provider containing the database context.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    public static async Task InitializeDatabaseAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SyncDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
