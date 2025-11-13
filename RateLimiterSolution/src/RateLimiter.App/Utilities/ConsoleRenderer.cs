using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RateLimiter.App.Models;

namespace RateLimiter.App.Utilities;

/// <summary>
/// Gestisce la presentazione su console di configurazioni, risultati e statistiche.
/// </summary>
public sealed class ConsoleRenderer
{
    /// <summary>
    /// Stampa la configurazione corrente del rate limiter.
    /// </summary>
    public void PrintConfig(RateLimiterOptions options)
    {
        Console.WriteLine("================ CONFIGURAZIONE ================");
        Console.WriteLine($"MaxConcurrentRequests: {options.MaxConcurrentRequests}");
        Console.WriteLine($"MaxRequestsPerWindow: {options.MaxRequestsPerWindow}");
        Console.WriteLine($"WindowLength: {options.WindowLength.TotalMilliseconds} ms");
        Console.WriteLine($"SimulatedWorkDuration: {options.SimulatedWorkDuration.TotalMilliseconds} ms");
        Console.WriteLine($"GlobalTimeout: {options.GlobalTimeout?.ToString() ?? "(nessun timeout)"}");
        Console.WriteLine();
    }

    /// <summary>
    /// Stampa in console i risultati dettagliati delle richieste processate.
    /// </summary>
    public void PrintResults(IEnumerable<RequestResult> results)
    {
        Console.WriteLine("================ RISULTATI DETTAGLIATI ================");
        foreach (var result in results)
        {
            var duration = result.EndTime - result.StartTime;
            Console.WriteLine(
                $"Richiesta {result.RequestId:D3} | Accettata: {result.Accepted} | Outcome: {result.Outcome} | Durata: {duration.TotalMilliseconds.ToString("F0", CultureInfo.InvariantCulture)} ms");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Stampa un riepilogo finale delle richieste processate.
    /// </summary>
    public void PrintSummary(IEnumerable<RequestResult> results)
    {
        var materialized = results.ToList();
        var total = materialized.Count;
        var accepted = materialized.Count(r => r.Accepted);
        var rejected = total - accepted;
        var acceptedDurations = materialized
            .Where(r => r.Accepted)
            .Select(r => (r.EndTime - r.StartTime).TotalMilliseconds)
            .Where(d => d >= 0)
            .ToList();

        var average = acceptedDurations.Count > 0
            ? acceptedDurations.Average()
            : 0d;

        Console.WriteLine("================ RIEPILOGO ================");
        Console.WriteLine($"Richieste totali: {total}");
        Console.WriteLine($"Accettate: {accepted}");
        Console.WriteLine($"Rifiutate: {rejected}");
        Console.WriteLine($"Durata media richieste accettate: {average.ToString("F2", CultureInfo.InvariantCulture)} ms");
        Console.WriteLine();
    }
}
