namespace AStar.Dev.OneDrive.Sync.Client.Core.Foundation;

/// <summary>
/// Provides a system-level or testable abstraction for system time.
/// </summary>
/// <remarks>
/// Built-in .NET TimeProvider supports switching between real system time and test fixtures via FakeTimeProvider.
/// </remarks>
public static class TimeProviderFactory
{
    /// <summary>
    /// Gets the system TimeProvider.
    /// In production, this is TimeProvider.System; tests can substitute FakeTimeProvider.
    /// </summary>
    public static TimeProvider Current { get; set; } = TimeProvider.System;
}
