namespace Pomodoro.App.Sessions;

public interface ISessionStrategy
{
    string Name { get; }
    (int focusSeconds, int breakSeconds) GetDurations();
}
