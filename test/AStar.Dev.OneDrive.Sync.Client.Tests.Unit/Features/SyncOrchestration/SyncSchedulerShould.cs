namespace AStar.Dev.OneDrive.Sync.Client.Tests.Unit.Features.SyncOrchestration;

using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.Accounts.Models;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.SyncOrchestration.Models;
using AStar.Dev.OneDrive.Sync.Client.Core.Foundation;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Shouldly;

/// <summary>
/// Tests for the sync scheduler's account eligibility and serial execution guarantees.
/// </summary>
public class SyncSchedulerShould
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISyncOrchestrator _orchestrator;
    private readonly FakeTimeProvider _timeProvider;
    private readonly SyncScheduler _scheduler;

    public SyncSchedulerShould()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _orchestrator = Substitute.For<ISyncOrchestrator>();
        _timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero));
        TimeProviderFactory.Current = _timeProvider;
        _scheduler = new SyncScheduler(_accountRepository, _orchestrator, _timeProvider);
    }

    [Fact]
    public async Task EnqueueAutoSyncAccountsAtStartup()
    {
        // Arrange
        var primaryAccount = new SyncAccount
        {
            Email = "primary@example.com",
            IsPrimary = true,
            AutoSyncEnabled = true,
            LastSyncAtUtc = null
        };
        var secondaryAccount = new SyncAccount
        {
            Email = "secondary@example.com",
            IsPrimary = false,
            AutoSyncEnabled = true,
            LastSyncAtUtc = null
        };

        _accountRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns([primaryAccount, secondaryAccount]);

        // Act
        await _scheduler.StartAsync(CancellationToken.None);

        // Assert
        await _orchestrator.Received(1).EnqueueAccountAsync(
            Arg.Is<AccountId>(id => id.Email == "primary@example.com"),
            Arg.Any<CancellationToken>());
        await _orchestrator.Received(1).EnqueueAccountAsync(
            Arg.Is<AccountId>(id => id.Email == "secondary@example.com"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task NotEnqueueAccountsWithoutAutoSync()
    {
        // Arrange
        var account = new SyncAccount
        {
            Email = "manual@example.com",
            IsPrimary = true,
            AutoSyncEnabled = false,
            LastSyncAtUtc = null
        };

        _accountRepository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns([account]);

        // Act
        await _scheduler.StartAsync(CancellationToken.None);

        // Assert
        await _orchestrator.DidNotReceive().EnqueueAccountAsync(
            Arg.Any<AccountId>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RejectPrimaryAccountSyncWithin15Minutes()
    {
        // Arrange
        var lastSync = _timeProvider.GetUtcNow().AddMinutes(-10);
        var account = new SyncAccount
        {
            Email = "primary@example.com",
            IsPrimary = true,
            LastSyncAtUtc = lastSync
        };

        _accountRepository.GetByEmailAsync(
            Arg.Is<AccountId>(id => id.Email == "primary@example.com"),
            Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _scheduler.CanSyncAccountAsync(new AccountId("primary@example.com"), CancellationToken.None);

        // Assert
        result.IsEligible.ShouldBeFalse();
        result.Reason.ShouldBe(IneligibilityReason.CadenceWindowNotMet);
    }

    [Fact]
    public async Task AllowPrimaryAccountSyncAfter15Minutes()
    {
        // Arrange
        var lastSync = _timeProvider.GetUtcNow().AddMinutes(-16);
        var account = new SyncAccount
        {
            Email = "primary@example.com",
            IsPrimary = true,
            LastSyncAtUtc = lastSync
        };

        _accountRepository.GetByEmailAsync(
            Arg.Is<AccountId>(id => id.Email == "primary@example.com"),
            Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _scheduler.CanSyncAccountAsync(new AccountId("primary@example.com"), CancellationToken.None);

        // Assert
        result.IsEligible.ShouldBeTrue();
    }

    [Fact]
    public async Task RejectSecondaryAccountSyncWithin1Hour()
    {
        // Arrange
        var lastSync = _timeProvider.GetUtcNow().AddMinutes(-30);
        var account = new SyncAccount
        {
            Email = "secondary@example.com",
            IsPrimary = false,
            LastSyncAtUtc = lastSync
        };

        _accountRepository.GetByEmailAsync(
            Arg.Is<AccountId>(id => id.Email == "secondary@example.com"),
            Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _scheduler.CanSyncAccountAsync(new AccountId("secondary@example.com"), CancellationToken.None);

        // Assert
        result.IsEligible.ShouldBeFalse();
        result.Reason.ShouldBe(IneligibilityReason.CadenceWindowNotMet);
    }

    [Fact]
    public async Task AllowSecondaryAccountSyncAfter1Hour()
    {
        // Arrange
        var lastSync = _timeProvider.GetUtcNow().AddMinutes(-61);
        var account = new SyncAccount
        {
            Email = "secondary@example.com",
            IsPrimary = false,
            LastSyncAtUtc = lastSync
        };

        _accountRepository.GetByEmailAsync(
            Arg.Is<AccountId>(id => id.Email == "secondary@example.com"),
            Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _scheduler.CanSyncAccountAsync(new AccountId("secondary@example.com"), CancellationToken.None);

        // Assert
        result.IsEligible.ShouldBeTrue();
    }

    [Fact]
    public async Task AllowFirstSyncForAccountWithNoHistory()
    {
        // Arrange
        var account = new SyncAccount
        {
            Email = "new@example.com",
            IsPrimary = true,
            LastSyncAtUtc = null
        };

        _accountRepository.GetByEmailAsync(
            Arg.Is<AccountId>(id => id.Email == "new@example.com"),
            Arg.Any<CancellationToken>())
            .Returns(account);

        // Act
        var result = await _scheduler.CanSyncAccountAsync(new AccountId("new@example.com"), CancellationToken.None);

        // Assert
        result.IsEligible.ShouldBeTrue();
    }

    [Fact]
    public async Task RejectSyncForNonExistentAccount()
    {
        // Arrange
        _accountRepository.GetByEmailAsync(
            Arg.Any<AccountId>(),
            Arg.Any<CancellationToken>())
            .Returns((SyncAccount?)null);

        // Act
        var result = await _scheduler.CanSyncAccountAsync(new AccountId("unknown@example.com"), CancellationToken.None);

        // Assert
        result.IsEligible.ShouldBeFalse();
        result.Reason.ShouldBe(IneligibilityReason.AccountNotFound);
    }
}
