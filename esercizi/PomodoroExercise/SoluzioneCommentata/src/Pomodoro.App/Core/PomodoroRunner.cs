namespace Pomodoro.App.Core;

public class PomodoroRunner
{
    private readonly Pomodoro _pomodoro;

    public PomodoroRunner(Pomodoro pomodoro)
    {
        _pomodoro = pomodoro;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // Il runner è volutamente minimale: coordina la UI di console e delega la
        // logica core al dominio. Questo permette di sostituirlo facilmente con una
        // UI diversa (desktop, web, ecc.).

        Console.WriteLine("Pomodoro Focus Timer — Soluzione commentata");
        Console.WriteLine("Premi CTRL+C per annullare.\n");

        // Mostriamo i tick rimanenti in console con un semplice aggiornamento.
        await _pomodoro.RunAsync(
            secondsRemaining =>
            {
                Console.WriteLine($"Secondi rimanenti: {secondsRemaining}");
            },
            cancellationToken);

        Console.WriteLine("Sessione completata!");
    }
}
