namespace AsincronaExamples.Examples;

/// <summary>
/// Esempio 5: Uso di IProgress per notificare l'avanzamento da un'operazione asincrona.
/// </summary>
public class ProgressReportingExample : IAsyncExample
{
    public string Name => "Avanzamento lavori";
    public string Description => "Mostra come aggiornare la UI (qui console) durante un'elaborazione.";

    public async Task RunAsync()
    {
        Console.WriteLine("\nESEMPIO 5 - Notifica di avanzamento\n");

        // Progress<T> consente di aggiornare la UI in modo thread-safe.
        var progress = new Progress<int>(percentuale =>
        {
            Console.CursorLeft = 0;
            Console.Write($"Avanzamento: {percentuale}%   ");
        });

        await SimulaElaborazioneAsync(progress);

        Console.WriteLine("\nElaborazione completata!\n");
    }

    private static async Task SimulaElaborazioneAsync(IProgress<int> progress)
    {
        const int step = 10;

        for (int percentuale = 0; percentuale <= 100; percentuale += step)
        {
            progress.Report(percentuale);
            await Task.Delay(200);
        }
    }
}
