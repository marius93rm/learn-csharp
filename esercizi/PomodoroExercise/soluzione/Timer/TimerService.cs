using System;
using System.Threading;
using System.Threading.Tasks;
using Pomodoro.App.Timer;

namespace Pomodoro.Exercise.Solution.Timer;

/// <summary>
/// Implementazione di riferimento per <see cref="Pomodoro.App.Timer.TimerService"/>.
/// La logica è volutamente compatta e commentata per mettere in evidenza come
/// il servizio si affidi esclusivamente a <see cref="ITickProvider"/>.
/// </summary>
public sealed class TimerServiceSolution
{
    private readonly ITickProvider _tickProvider;

    public TimerServiceSolution(ITickProvider tickProvider)
    {
        _tickProvider = tickProvider;
    }

    /// <summary>
    /// Esegue un conto alla rovescia notificando il tempo rimanente.
    /// L'uso di <paramref name="_tickProvider"/> rende il servizio facilmente
    /// testabile (ad esempio fornendo un provider che non attende realmente).
    /// </summary>
    public async Task CountdownAsync(
        int totalSeconds,
        Action<int>? onTick,
        Action? onCompleted,
        CancellationToken cancellationToken = default)
    {
        if (totalSeconds < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalSeconds));
        }

        // Gestiamo in modo esplicito il caso "0 secondi": nessun tick, ma chiamiamo subito onCompleted.
        if (totalSeconds == 0)
        {
            onCompleted?.Invoke();
            return;
        }

        for (var remainingSeconds = totalSeconds; remainingSeconds > 0; remainingSeconds--)
        {
            // Notifichiamo il chiamante con i secondi rimanenti.
            onTick?.Invoke(remainingSeconds);

            // L'attesa effettiva è delegata al provider, così da poter usare una finta implementazione nei test.
            await _tickProvider.DelayAsync(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);
        }

        // Al termine del countdown segnaliamo il completamento.
        onCompleted?.Invoke();
    }
}
