using Pomodoro.App.Notify;
using Pomodoro.App.Persistence;
using Pomodoro.App.Sessions;
using Pomodoro.App.Timer;

namespace Pomodoro.App.Core;

public class Pomodoro
{
    private readonly TimerService _timerService;
    private readonly ISessionStrategy _strategy;
    private readonly IReadOnlyCollection<INotifier> _notifiers;
    private readonly ISessionRepository _repository;

    public Pomodoro(
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

    public async Task RunAsync(Action<int>? onTick = null, CancellationToken cancellationToken = default)
    {
        // La strategia incapsula le durate dei cicli e rispetta OCP permettendo
        // di aggiungerne di nuove senza toccare questa classe.
        var (focusSeconds, breakSeconds) = _strategy.GetDurations();

        // Fase di focus: instradiamo i tick verso l'eventuale callback fornita
        // dall'interfaccia utente (es. la console). In questo modo la logica di
        // presentazione rimane separata dal dominio.
        await _timerService.CountdownAsync(
            focusSeconds,
            seconds => onTick?.Invoke(seconds),
            onCompleted: () =>
            {
                // Una volta terminata la fase di focus notifichiamo gli
                // osservatori e pubblichiamo l'evento.
                foreach (var notifier in _notifiers)
                {
                    notifier.Notify(
                        $"Focus completato! Ora prendi una pausa di {TimeSpan.FromSeconds(breakSeconds):mm\:ss}.");
                }

                OnFocusCompleted();
            },
            cancellationToken);

        // Se è previsto un break lo eseguiamo. I tick non vengono inoltrati
        // all'esterno per mantenere l'esempio focalizzato sul focus.
        if (breakSeconds > 0)
        {
            await _timerService.CountdownAsync(
                breakSeconds,
                _ => { },
                onCompleted: () =>
                {
                    foreach (var notifier in _notifiers)
                    {
                        notifier.Notify("Pausa terminata! Preparati per il prossimo ciclo.");
                    }
                },
                cancellationToken);
        }

        // Persistiamo la sessione in modo che un eventuale UI possa mostrare
        // uno storico delle attività completate.
        var session = new PomodoroSessionLog(
            StartedAt: DateTime.UtcNow,
            FocusSeconds: focusSeconds,
            BreakSeconds: breakSeconds,
            StrategyName: _strategy.Name);

        await _repository.SaveAsync(session, cancellationToken);
    }

    protected virtual void OnFocusCompleted()
    {
        FocusCompleted?.Invoke(this, EventArgs.Empty);
    }
}
