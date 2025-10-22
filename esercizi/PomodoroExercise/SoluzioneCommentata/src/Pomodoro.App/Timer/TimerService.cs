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

        // Il timer è volutamente espresso in secondi così da poter essere utilizzato
        // anche da un provider "finto" (come quello usato dai test) che restituisce
        // immediatamente il controllo senza attendere davvero. Per questo motivo non
        // usiamo `Task.Delay` direttamente, ma deleghiamo l'attesa al provider
        // iniettato che rispetta la DIP e rende il servizio facilmente testabile.

        for (var remaining = totalSeconds; remaining > 0; remaining--)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Notifichiamo il numero di secondi residui prima di effettuare la pausa.
            onTick?.Invoke(remaining);

            // Il provider di tick decide come attendere un secondo. Nei test viene
            // restituito immediatamente, mentre in produzione attende davvero.
            await _tickProvider.DelayAsync(TimeSpan.FromSeconds(1), cancellationToken)
                .ConfigureAwait(false);
        }

        // Al termine del countdown richiamiamo la callback di completamento. Non
        // solleviamo eccezioni se `onCompleted` è nullo: l'operatore `?.` è una scelta
        // sicura per una demo da usare durante l'esercitazione.
        onCompleted?.Invoke();
    }
}
