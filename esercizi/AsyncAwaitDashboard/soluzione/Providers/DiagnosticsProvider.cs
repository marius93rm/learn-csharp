using AsyncAwaitDashboard.App.Infrastructure;

namespace AsyncAwaitDashboard.App.Providers;

/// <summary>
/// Mappa il payload grezzo dello stato di sistema nella forma sintetica usata dal dashboard.
/// </summary>
public class DiagnosticsProvider : IDiagnosticsProvider
{
    private readonly SimulatedApiClient _apiClient;

    public DiagnosticsProvider(SimulatedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<SystemStatusSummary> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        // Recuperiamo il payload asincrono dal client simulato.
        var payload = await _apiClient.GetSystemStatusAsync(cancellationToken).ConfigureAwait(false);

        // Convertiamo i dati nell'oggetto di dominio usato dalla dashboard.
        return new SystemStatusSummary(payload.PendingJobs, payload.LastBackupAge, payload.IsDegraded);
    }

    public IAsyncEnumerable<DiagnosticEvent> StreamLiveEventsAsync(CancellationToken cancellationToken = default)
        => _apiClient.StreamDiagnosticsAsync(cancellationToken);
}
