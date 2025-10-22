namespace Pomodoro.App.Timer;

public class RealTickProvider : ITickProvider
{
    public Task DelayAsync(TimeSpan interval, CancellationToken cancellationToken)
    {
        return Task.Delay(interval, cancellationToken);
    }
}
