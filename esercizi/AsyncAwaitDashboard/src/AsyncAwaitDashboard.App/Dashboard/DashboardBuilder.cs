using System.Runtime.CompilerServices;
using AsyncAwaitDashboard.App.Providers;

namespace AsyncAwaitDashboard.App.Dashboard;

public class DashboardBuilder
{
    private readonly IWeatherProvider _weatherProvider;
    private readonly IQuoteProvider _quoteProvider;
    private readonly IDiagnosticsProvider _diagnosticsProvider;

    public DashboardBuilder(
        IWeatherProvider weatherProvider,
        IQuoteProvider quoteProvider,
        IDiagnosticsProvider diagnosticsProvider)
    {
        _weatherProvider = weatherProvider;
        _quoteProvider = quoteProvider;
        _diagnosticsProvider = diagnosticsProvider;
    }

    public async Task<DashboardSnapshot> BuildAsync(CancellationToken cancellationToken = default)
    {
        // TODO [M3]: avvia le chiamate ai provider in parallelo (Task.WhenAll) e costruisci la snapshot.
        // Suggerimento: chiama `GetCurrentAsync`, `GetQuoteAsync`, `GetStatusAsync` prima di `await`.
        throw new NotImplementedException("TODO [M3]: implementa l'aggregazione asincrona.");
    }

    public async IAsyncEnumerable<DiagnosticEvent> StreamDiagnosticsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // TODO [M4]: propaga lo stream dell'IDiagnosticsProvider aggiungendo gestione cancellation.
        // Suggerimento: `await foreach` + `yield return`.
        throw new NotImplementedException("TODO [M4]: stream asincrono da completare.");
    }
}
