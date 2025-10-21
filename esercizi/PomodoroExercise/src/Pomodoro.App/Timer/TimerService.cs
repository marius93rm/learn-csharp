namespace Pomodoro.App.Timer;

public class TimerService
{
    private readonly ITickProvider _tickProvider;

    public TimerService(ITickProvider tickProvider)
    {
        _tickProvider = tickProvider;
    }

    public async Task CountdownAsync(
        int totalSeconds,
        Action<int> onTick,
        Action onCompleted,
        CancellationToken cancellationToken = default)
    {
        if (totalSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSeconds));
        }

        // TODO: implementare il countdown utilizzando ITickProvider per scandire i secondi.
        // Suggerimento: decrementa il tempo e invoca onTick per ogni secondo rimanente.
        // Al termine, invoca onCompleted.
        throw new NotImplementedException("TODO: CountdownAsync deve essere implementato dagli studenti.");
    }
}
