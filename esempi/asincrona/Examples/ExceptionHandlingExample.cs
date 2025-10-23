namespace AsincronaExamples.Examples;

/// <summary>
/// Esempio 4: Gestione delle eccezioni all'interno di metodi asincroni.
/// </summary>
public class ExceptionHandlingExample : IAsyncExample
{
    public string Name => "Gestione eccezioni";
    public string Description => "Dimostra come intercettare eccezioni provenienti da Task.";

    public async Task RunAsync()
    {
        Console.WriteLine("\nESEMPIO 4 - Gestione delle eccezioni asincrone\n");
        Console.WriteLine("Forziamo un errore in un metodo asincrono e gestiamolo elegantemente.\n");

        try
        {
            await MetodoCheGeneraErroreAsync();
            Console.WriteLine("Questo messaggio non verrà mai eseguito perché sopra viene lanciata un'eccezione.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Abbiamo intercettato una InvalidOperationException!\n");
            Console.WriteLine($"Messaggio originale: {ex.Message}");
            Console.WriteLine("Stack trace asincrono preservato automaticamente da .NET.");
        }
        finally
        {
            Console.WriteLine("Blocco finally eseguito: ottimo posto per rilasciare risorse.");
        }
    }

    private static async Task MetodoCheGeneraErroreAsync()
    {
        await Task.Delay(500);
        throw new InvalidOperationException("Qualcosa è andato storto durante l'elaborazione asincrona.");
    }
}
