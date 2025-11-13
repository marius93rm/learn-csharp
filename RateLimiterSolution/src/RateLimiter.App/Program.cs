using System;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.App.Models;
using RateLimiter.App.Services;
using RateLimiter.App.Utilities;

namespace RateLimiter.App;

internal static class Program
{
    /// <summary>
    /// Entry point asincrono dell'applicazione console.
    /// </summary>
    private static async Task Main(string[] args)
    {
        var options = new RateLimiterOptions();
        var rateLimiter = new RateLimiter(options);
        var simulator = new RequestSimulator(rateLimiter, options);
        var renderer = new ConsoleRenderer();

        renderer.PrintConfig(options);

        Console.Write("Quante richieste simulare? (default 25): ");
        var totalInput = Console.ReadLine();
        if (!int.TryParse(totalInput, out var totalRequests) || totalRequests <= 0)
        {
            totalRequests = 25;
        }

        Console.Write("Mostrare risultati dettagliati? (s/N): ");
        var showDetailsInput = Console.ReadLine();
        var showDetails = string.Equals(showDetailsInput, "s", StringComparison.OrdinalIgnoreCase);

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cts.Cancel();
            Console.WriteLine();
            Console.WriteLine("Interruzione richiesta. Attendere la chiusura delle attività in corso...");
        };

        try
        {
            var results = await simulator.RunBatchAsync(totalRequests, cts.Token).ConfigureAwait(false);

            if (showDetails)
            {
                renderer.PrintResults(results);
            }

            renderer.PrintSummary(results);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operazione annullata dall'utente.");
        }

        Console.WriteLine("Premere INVIO per uscire.");
        Console.ReadLine();
    }
}
