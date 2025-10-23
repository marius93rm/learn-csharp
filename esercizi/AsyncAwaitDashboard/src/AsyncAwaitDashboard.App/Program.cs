using AsyncAwaitDashboard.App.Dashboard;
using AsyncAwaitDashboard.App.Infrastructure;
using AsyncAwaitDashboard.App.Providers;

var apiClient = new SimulatedApiClient(new NetworkLatencySimulator());
var weatherProvider = new WeatherProvider(apiClient);
var quoteProvider = new QuoteProvider(apiClient);
var diagnosticsProvider = new DiagnosticsProvider(apiClient);

var builder = new DashboardBuilder(weatherProvider, quoteProvider, diagnosticsProvider);

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

Console.WriteLine("== Async Await Dashboard ==");

// TODO [M5]: usa await per richiamare BuildAsync e stampa il risultato in console in modo leggibile.
// Suggerimento: il tipo `DashboardSnapshot` espone `ToConsoleString()` per formattare i dati.
_ = await builder.BuildAsync(cts.Token);

Console.WriteLine("\n== Diagnostica live (max 5 eventi) ==");

// TODO [M5]: itera `StreamDiagnosticsAsync` con `await foreach` e interrompi dopo 5 elementi.
await foreach (var _ in builder.StreamDiagnosticsAsync(cts.Token))
{
    break;
}
