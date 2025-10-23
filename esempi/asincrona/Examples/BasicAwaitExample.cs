using System.Diagnostics;

namespace AsincronaExamples.Examples;

/// <summary>
/// Esempio 1: Introduzione ad async/await con un'operazione simulata che richiede tempo.
/// </summary>
public class BasicAwaitExample : IAsyncExample
{
    public string Name => "Async/Await di base";
    public string Description => "Mostra come attendere in modo asincrono un'operazione I/O simulata.";

    public async Task RunAsync()
    {
        Console.WriteLine("\nESEMPIO 1 - Async/Await di base\n");
        Console.WriteLine("Simuliamo una chiamata a servizio esterno usando Task.Delay...");

        var stopwatch = Stopwatch.StartNew();

        // Task.Delay simula un'operazione I/O lunga (ad es. richiesta HTTP o accesso al database)
        string risultato = await RecuperaDatiDaServizioAsync();

        stopwatch.Stop();

        Console.WriteLine($"Risultato ricevuto: {risultato}");
        Console.WriteLine($"Tempo totale impiegato: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine("\nNota: durante l'attesa il thread non viene bloccato, permettendo all'app di rimanere reattiva.\n");
    }

    private static async Task<string> RecuperaDatiDaServizioAsync()
    {
        // In una app reale qui avremmo ad esempio HttpClient.GetAsync.
        await Task.Delay(1500);
        return "Dati recuperati in modo asincrono";
    }
}
