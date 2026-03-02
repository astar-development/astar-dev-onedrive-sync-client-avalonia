namespace AStar.Dev.OneDrive.Sync.Client.Tests.Unit.Features.DeltaProcessing;

using AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing;
using AStar.Dev.OneDrive.Sync.Client.Core.Features.DeltaProcessing.Models;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Shouldly;

/// <summary>
/// Tests for Graph API client's rate limiting and retry behavior.
/// </summary>
public class GraphDeltaClientShould
{
    private readonly IGraphApiAdapter _graphAdapter;
    private readonly FakeTimeProvider _timeProvider;
    private readonly GraphDeltaClient _client;

    public GraphDeltaClientShould()
    {
        _graphAdapter = Substitute.For<IGraphApiAdapter>();
        _timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 3, 2, 10, 0, 0, TimeSpan.Zero));
        _client = new GraphDeltaClient(_graphAdapter, _timeProvider);
    }

    [Fact]
    public async Task RetryOn429WithRetryAfterHeader()
    {
        // Arrange
        var accountEmail = "test@example.com";
        var successResponse = new DeltaResponse
        {
            Items = [],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=success"
        };

        var callCount = 0;

        _graphAdapter.GetDeltaAsync(accountEmail, null, Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                callCount++;
                if (callCount == 1)
                {
                    throw new GraphRateLimitException(TimeSpan.FromSeconds(2));
                }
                return Task.FromResult(successResponse);
            });

        // Act
        var result = await _client.GetInitialDeltaAsync(accountEmail, CancellationToken.None);

        // Assert
        callCount.ShouldBe(2);
        result.DeltaLink.ShouldContain("success");
    }

    [Fact]
    public async Task UseExponentialBackoffWhenRetryAfterNotProvided()
    {
        // Arrange
        var accountEmail = "test@example.com";
        var successResponse = new DeltaResponse
        {
            Items = [],
            DeltaLink = "https://graph.microsoft.com/v1.0/delta?token=success"
        };

        var callCount = 0;

        _graphAdapter.GetDeltaAsync(accountEmail, null, Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                callCount++;
                if (callCount <= 2)
                {
                    throw new GraphRateLimitException(null);
                }
                return Task.FromResult(successResponse);
            });

        // Act
        var result = await _client.GetInitialDeltaAsync(accountEmail, CancellationToken.None);

        // Assert
        callCount.ShouldBe(3);
        result.DeltaLink.ShouldContain("success");
    }

    [Fact]
    public async Task FailAfterMaxRetryAttempts()
    {
        // Arrange
        var accountEmail = "test@example.com";

        _graphAdapter.GetDeltaAsync(accountEmail, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<DeltaResponse>(new GraphRateLimitException(TimeSpan.FromSeconds(1))));

        // Act & Assert
        await Should.ThrowAsync<GraphRateLimitException>(
            async () => await _client.GetInitialDeltaAsync(accountEmail, CancellationToken.None));
    }

    [Fact]
    public async Task PropagateNonRateLimitExceptionsImmediately()
    {
        // Arrange
        var accountEmail = "test@example.com";

        _graphAdapter.GetDeltaAsync(accountEmail, null, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<DeltaResponse>(new InvalidOperationException("Network error")));

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(
            async () => await _client.GetInitialDeltaAsync(accountEmail, CancellationToken.None));
    }

    [Fact]
    public async Task RespectCancellationTokenDuringRetry()
    {
        // Arrange
        var accountEmail = "test@example.com";
        var cts = new CancellationTokenSource();

        _graphAdapter.GetDeltaAsync(accountEmail, null, Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                cts.Cancel();
                return Task.FromException<DeltaResponse>(new GraphRateLimitException(TimeSpan.FromSeconds(10)));
            });

        // Act & Assert
        await Should.ThrowAsync<OperationCanceledException>(
            async () => await _client.GetInitialDeltaAsync(accountEmail, cts.Token));
    }
}
