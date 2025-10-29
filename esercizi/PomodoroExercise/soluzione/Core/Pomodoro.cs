using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pomodoro.App.Core;
using Pomodoro.App.Notify;
using Pomodoro.App.Persistence;
using Pomodoro.App.Sessions;
using Pomodoro.App.Timer;

namespace Pomodoro.Exercise.Solution.Core;

/// <summary>
/// Esempio di implementazione per la classe <see cref="Pomodoro.App.Core.Pomodoro"/>.
/// Gli step seguono i suggerimenti del README dell'esercizio e sono annotati per
/// chiarire le responsabilità coinvolte.
/// </summary>
public sealed class PomodoroSolution
{
    private readonly TimerService _timerService;
    private readonly ISessionStrategy _strategy;
    private readonly IReadOnlyCollection<INotifier> _notifiers;
    private readonly ISessionRepository _repository;

    public PomodoroSolution(
        TimerService timerService,
        ISessionStrategy strategy,
        IEnumerable<INotifier> notifiers,
        ISessionRepository repository)
    {
        _timerService = timerService;
        _strategy = strategy;
        _notifiers = notifiers.ToList();
        _repository = repository;
    }

    public event EventHandler? FocusCompleted;

    /// <summary>
    /// Orchestri la sequenza focus → break rispettando il DIP: tutte le collaborazioni
    /// arrivano dall'esterno (timer, strategia, notifiers, repository).
    /// </summary>
    public async Task RunAsync(Action<int>? onTick = null, CancellationToken cancellationToken = default)
    {
        // 1. Interroghiamo la strategia per conoscere le durate configurate.
        var (focusSeconds, breakSeconds) = _strategy.GetDurations();

        // Conserviamo il timestamp per il log prima che inizi il focus.
        var startedAt = DateTime.UtcNow;

        var focusCompleted = false;

        // 2. Avviamo il countdown di focus delegando il "passare del tempo" al TimerService.
        await _timerService.CountdownAsync(
            focusSeconds,
            remainingSeconds => onTick?.Invoke(remainingSeconds),
            () => focusCompleted = true,
            cancellationToken).ConfigureAwait(false);

        if (focusCompleted)
        {
            // 3. Notifichiamo tutti gli osservatori e pubblichiamo l'evento.
            foreach (var notifier in _notifiers)
            {
                notifier.Notify($"Focus '{_strategy.Name}' completato. È il momento della pausa!");
            }

            FocusCompleted?.Invoke(this, EventArgs.Empty);
        }

        // 4. Avviamo eventualmente il break. Non è necessario inoltrare i tick del break,
        //    quindi passiamo una callback vuota.
        if (breakSeconds > 0)
        {
            await _timerService.CountdownAsync(
                breakSeconds,
                _ => { },
                () => { },
                cancellationToken).ConfigureAwait(false);
        }

        // 5. Persistiamo la sessione nel repository: SRP rispettato perché la classe
        //    delega all'astrazione l'effettivo salvataggio.
        var session = new PomodoroSessionLog(
            startedAt,
            focusSeconds,
            breakSeconds,
            _strategy.Name);

        await _repository.SaveAsync(session, cancellationToken).ConfigureAwait(false);
    }
}
