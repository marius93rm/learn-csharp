using AsyncAwaitDashboard.App.Infrastructure;

namespace AsyncAwaitDashboard.App.Providers;

public class DiagnosticsProvider : IDiagnosticsProvider
{
    private readonly SimulatedApiClient _apiClient;

    public DiagnosticsProvider(SimulatedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<SystemStatusSummary> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        // TODO [M2]: Recupera lo stato del sistema dal client simulato e convertilo in SystemStatusSummary.
        throw new NotImplementedException("TODO [M2]: implementa il recupero stato sistema.");
    }

    public IAsyncEnumerable<DiagnosticEvent> StreamLiveEventsAsync(CancellationToken cancellationToken = default)
        => _apiClient.StreamDiagnosticsAsync(cancellationToken);
}
