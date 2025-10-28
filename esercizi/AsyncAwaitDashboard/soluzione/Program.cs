using AsyncAwaitDashboard.App.Dashboard;
using AsyncAwaitDashboard.App.Infrastructure;
using AsyncAwaitDashboard.App.Providers;

// Programma di esempio completamente funzionante che mostra come usare gli
// oggetti dell'esercizio originale senza toccare i file del progetto.
// Tutti i passaggi sono commentati per facilitare lo studio della soluzione.
var apiClient = new SimulatedApiClient(new NetworkLatencySimulator());
var weatherProvider = new WeatherProvider(apiClient);
var quoteProvider = new QuoteProvider(apiClient);
var diagnosticsProvider = new DiagnosticsProvider(apiClient);

var builder = new DashboardBuilder(weatherProvider, quoteProvider, diagnosticsProvider);

// Creiamo un CancellationTokenSource così da poter interrompere sia la
// costruzione del dashboard sia lo stream degli eventi di diagnostica.
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

Console.WriteLine("== Async Await Dashboard (soluzione) ==");

// 1) Avviamo la costruzione del dashboard usando await. Il metodo restituisce
//    un DashboardSnapshot già pronto per essere stampato.
var snapshot = await builder.BuildAsync(cts.Token);

// 2) Il record DashboardSnapshot mette a disposizione ToConsoleString per
//    ottenere un riepilogo formattato in maniera leggibile.
Console.WriteLine(snapshot.ToConsoleString());

Console.WriteLine("\n== Diagnostica live (primi 5 eventi) ==");

// 3) Usiamo await foreach per consumare lo stream asincrono di eventi.
//    Manteniamo un contatore per fermarci dopo aver stampato cinque messaggi.
var count = 0;
await foreach (var diagnostic in builder.StreamDiagnosticsAsync(cts.Token))
{
    Console.WriteLine($"[{diagnostic.Timestamp:HH:mm:ss}] {diagnostic.Severity,-5} {diagnostic.Message}");

    count++;
    if (count >= 5)
    {
        break; // interrompiamo manualmente dopo il quinto evento.
    }
}
