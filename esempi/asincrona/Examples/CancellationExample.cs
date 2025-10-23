using System.Diagnostics;
using System.Threading;

namespace AsincronaExamples.Examples;

/// <summary>
/// Esempio 3: Gestione della cancellazione con CancellationToken.
/// </summary>
public class CancellationExample : IAsyncExample
{
    public string Name => "Cancellazione";
    public string Description => "Mostra come interrompere un'operazione lunga su richiesta dell'utente.";

    public async Task RunAsync()
    {
        Console.WriteLine("\nESEMPIO 3 - Cancellazione di un'operazione asincrona\n");
        Console.WriteLine("Premi INVIO entro 3 secondi per annullare il conteggio progressivo...");

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(3)); // Cancella automaticamente se l'utente non interviene in tempo.

        // Avvia un task in background che attende l'input dell'utente e cancella l'operazione.
        var inputTask = Task.Run(() =>
        {
            Console.ReadLine();
            cts.Cancel();
        });

        try
        {
            await ContaConCancellazioneAsync(cts.Token);
            Console.WriteLine("\nOperazione completata senza cancellazione.\n");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nOperazione annullata! (da timeout o da input dell'utente)\n");
        }
    }

    private static async Task ContaConCancellazioneAsync(CancellationToken token)
    {
        var stopwatch = Stopwatch.StartNew();

        for (int i = 1; i <= 10; i++)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine($"Passo {i}/10");
            await Task.Delay(500, token); // La delay osserva il token e può essere interrotta.
        }

        stopwatch.Stop();
        Console.WriteLine($"Conteggio completato in {stopwatch.ElapsedMilliseconds} ms");
    }
}
