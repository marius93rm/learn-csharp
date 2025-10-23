using AsincronaExamples.Examples;

namespace AsincronaExamples;

/// <summary>
/// Entry point dell'applicazione console. Mostra un menù interattivo che permette di
/// eseguire i vari esempi di programmazione asincrona presenti nella cartella Examples.
/// </summary>
public static class Program
{
    private static readonly IReadOnlyList<IAsyncExample> Examples = new List<IAsyncExample>
    {
        new BasicAwaitExample(),
        new ParallelRequestsExample(),
        new CancellationExample(),
        new ExceptionHandlingExample(),
        new ProgressReportingExample()
    };

    public static async Task Main()
    {
        Console.Title = "Esempi di programmazione asincrona";
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            StampaMenu();
            Console.Write("Seleziona un'opzione: ");
            string? input = Console.ReadLine();

            if (string.Equals(input, "0", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nUscita dal programma. A presto!");
                return;
            }

            if (int.TryParse(input, out int scelta) && scelta >= 1 && scelta <= Examples.Count)
            {
                Console.Clear();
                var example = Examples[scelta - 1];
                Console.WriteLine($"Hai scelto: {example.Name}");
                Console.WriteLine(example.Description);

                await example.RunAsync();

                Console.WriteLine("Premi INVIO per tornare al menù principale...");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Scelta non valida. Premi INVIO per riprovare...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    private static void StampaMenu()
    {
        Console.WriteLine("==============================");
        Console.WriteLine(" Esempi di Programmazione Asincrona");
        Console.WriteLine("==============================\n");
        Console.WriteLine("Scegli quale scenario vuoi esplorare (0 o Q per uscire):\n");

        for (int i = 0; i < Examples.Count; i++)
        {
            Console.WriteLine($" {i + 1}) {Examples[i].Name} - {Examples[i].Description}");
        }

        Console.WriteLine("\n 0) Esci");
        Console.WriteLine(" Q) Esci\n");
    }
}
