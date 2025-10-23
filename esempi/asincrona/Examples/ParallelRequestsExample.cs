namespace AsincronaExamples.Examples;

/// <summary>
/// Esempio 2: esecuzione parallela di più operazioni asincrone con Task.WhenAll.
/// </summary>
public class ParallelRequestsExample : IAsyncExample
{
    public string Name => "Operazioni parallele";
    public string Description => "Esegue più chiamate in parallelo e ne confronta i tempi.";

    public async Task RunAsync()
    {
        Console.WriteLine("\nESEMPIO 2 - Operazioni asincrone in parallelo\n");
        Console.WriteLine("Avviamo tre operazioni lente in parallelo...");

        var operazioni = new[]
        {
            SimulaOperazioneAsync("Operazione A", 1200),
            SimulaOperazioneAsync("Operazione B", 2200),
            SimulaOperazioneAsync("Operazione C", 1800)
        };

        // Task.WhenAll avvia tutte le task e aspetta che siano completate.
        string[] risultati = await Task.WhenAll(operazioni);

        Console.WriteLine("\nTutte le operazioni sono terminate. Riassunto:");
        foreach (var risultato in risultati)
        {
            Console.WriteLine(" - " + risultato);
        }

        Console.WriteLine("\nNota: il tempo totale corrisponde alla task più lunga, non alla somma di tutte.\n");
    }

    private static async Task<string> SimulaOperazioneAsync(string nome, int durataMs)
    {
        Console.WriteLine($"{nome} avviata (durata stimata: {durataMs} ms)...");
        await Task.Delay(durataMs);
        Console.WriteLine($"{nome} completata.");
        return $"{nome} completata in {durataMs} ms";
    }
}
