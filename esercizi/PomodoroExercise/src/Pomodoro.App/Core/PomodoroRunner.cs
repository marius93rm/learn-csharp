namespace Pomodoro.App.Core;

public class PomodoroRunner
{
    private readonly Pomodoro _pomodoro;

    public PomodoroRunner(Pomodoro pomodoro)
    {
        _pomodoro = pomodoro;
    }

    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        // TODO: orchestrare l'interazione con l'utente e chiamare Pomodoro.RunAsync.
        // Suggerimento: mostra le durate e avvia il timer.
        throw new NotImplementedException("TODO: PomodoroRunner.RunAsync deve essere implementato dagli studenti.");
    }
}
