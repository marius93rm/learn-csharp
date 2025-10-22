namespace Pomodoro.App.Sessions;

public class Deep50_10 : ISessionStrategy
{
    public string Name => "Deep 50/10";

    public (int focusSeconds, int breakSeconds) GetDurations()
    {
        return (50 * 60, 10 * 60);
    }
}
