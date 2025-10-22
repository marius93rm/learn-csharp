namespace Pomodoro.App.Persistence;

public record PomodoroSessionLog(
    DateTime StartedAt,
    int FocusSeconds,
    int BreakSeconds,
    string StrategyName);

public interface ISessionRepository
{
    Task SaveAsync(PomodoroSessionLog session, CancellationToken cancellationToken = default);
}
