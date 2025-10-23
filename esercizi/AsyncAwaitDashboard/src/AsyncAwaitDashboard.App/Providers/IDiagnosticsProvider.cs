namespace AsyncAwaitDashboard.App.Providers;

public interface IDiagnosticsProvider
{
    Task<SystemStatusSummary> GetStatusAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<DiagnosticEvent> StreamLiveEventsAsync(
        CancellationToken cancellationToken = default);
}

public sealed record SystemStatusSummary(int PendingJobs, TimeSpan LastBackupAge, bool IsDegraded);

public sealed record DiagnosticEvent(DateTime Timestamp, string Severity, string Message);
