using System.Runtime.CompilerServices;
using AsyncAwaitDashboard.App.Providers;

namespace AsyncAwaitDashboard.App.Dashboard;

/// <summary>
/// Versione risolta di <see cref="DashboardBuilder"/>: mette in parallelo i provider
/// e offre uno stream di diagnostica già pronto da consumare.
/// </summary>
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

    /// <summary>
    /// Recupera meteo, quotazioni e stato di sistema avviando le tre richieste in parallelo.
    /// </summary>
    public async Task<DashboardSnapshot> BuildAsync(CancellationToken cancellationToken = default)
    {
        // Avviamo le tre chiamate prima di attenderle: in questo modo il tempo totale
        // corrisponde alla richiesta più lenta.
        var weatherTask = _weatherProvider.GetCurrentAsync(cancellationToken);
        var quoteTask = _quoteProvider.GetQuoteAsync(cancellationToken);
        var statusTask = _diagnosticsProvider.GetStatusAsync(cancellationToken);

        // Quando tutte le operazioni sono concluse costruiamo l'istantanea del dashboard.
        await Task.WhenAll(weatherTask, quoteTask, statusTask).ConfigureAwait(false);

        return new DashboardSnapshot(
            await weatherTask.ConfigureAwait(false),
            await quoteTask.ConfigureAwait(false),
            await statusTask.ConfigureAwait(false));
    }

    /// <summary>
    /// Espone lo stream asincrono di diagnostica propagando la cancellazione al chiamante.
    /// </summary>
    public async IAsyncEnumerable<DiagnosticEvent> StreamDiagnosticsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // `await foreach` consente di inoltrare gli eventi uno per volta mantenendo
        // l'operazione completamente asincrona.
        await foreach (var diagnostic in _diagnosticsProvider
                           .StreamLiveEventsAsync(cancellationToken)
                           .WithCancellation(cancellationToken)
                           .ConfigureAwait(false))
        {
            yield return diagnostic;
        }
    }
}
