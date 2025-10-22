namespace Pomodoro.App.Sessions;

public class Classic25_5 : ISessionStrategy
{
    public string Name => "Classic 25/5";

    public (int focusSeconds, int breakSeconds) GetDurations()
    {
        return (25 * 60, 5 * 60);
    }
}
