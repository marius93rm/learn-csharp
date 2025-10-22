using Pomodoro.App.Core;

using Pomodoro.App.Notify;
using Pomodoro.App.Persistence;
using Pomodoro.App.Sessions;
using Pomodoro.App.Timer;

namespace Pomodoro.App;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("🕒 Pomodoro Focus Timer — modalità soluzione commentata");

        // Configuriamo le dipendenze principali del dominio. Il provider di tick è
        // la dipendenza fondamentale del timer e viene iniettato nel servizio.
        var tickProvider = new RealTickProvider();
        var timerService = new TimerService(tickProvider);

        // Per l'esempio scegliamo la strategia classica 25/5 ma nulla vieta di
        // renderla configurabile da input utente.
        ISessionStrategy strategy = new Classic25_5();

        // I notifier sono componenti che rispettano l'ISP: ciascuno implementa un
        // singolo metodo. Qui combiniamo console e beep.
        var notifiers = new INotifier[]
        {
            new ConsoleNotifier(),
            new BeepNotifier(),
        };

        // Salviamo lo storico delle sessioni in un file CSV nella cartella
        // dell'applicazione.
        var logPath = Path.Combine(AppContext.BaseDirectory, "sessions.csv");
        var repository = new CsvSessionRepository(logPath);

        var pomodoro = new Core.Pomodoro(timerService, strategy, notifiers, repository);
        var runner = new PomodoroRunner(pomodoro);

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            Console.WriteLine("\nAnnullamento richiesto, chiusura in corso...");
            eventArgs.Cancel = true;
            cts.Cancel();
        };

        try
        {
            await runner.RunAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Sessione annullata.");
        }
    }
}
