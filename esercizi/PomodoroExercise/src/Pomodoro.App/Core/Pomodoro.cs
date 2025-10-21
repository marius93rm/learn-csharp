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

    public Task RunAsync(Action<int>? onTick = null, CancellationToken cancellationToken = default)
    {
        // TODO: orchestrare focus e break usando TimerService.
        // 1. Ottieni le durate dalla strategia.
        // 2. Avvia il countdown di focus e notifica al termine.
        // 3. Pubblica l'evento FocusCompleted.
        // 4. Avvia il break e persisti la sessione usando il repository.
        throw new NotImplementedException("TODO: Pomodoro.RunAsync deve essere implementato dagli studenti.");
    }

    protected virtual void OnFocusCompleted()
    {
        FocusCompleted?.Invoke(this, EventArgs.Empty);
    }
}
