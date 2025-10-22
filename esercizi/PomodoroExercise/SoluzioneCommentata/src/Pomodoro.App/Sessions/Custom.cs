namespace Pomodoro.App.Sessions;

public class Custom : ISessionStrategy
{
    public Custom(string name, int focusSeconds, int breakSeconds)
    {
        Name = name;
        FocusSeconds = focusSeconds;
        BreakSeconds = breakSeconds;
    }

    public string Name { get; }

    public int FocusSeconds { get; }

    public int BreakSeconds { get; }

    public (int focusSeconds, int breakSeconds) GetDurations()
    {
        return (FocusSeconds, BreakSeconds);
    }
}
