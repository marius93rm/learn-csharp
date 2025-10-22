namespace Pomodoro.App.Timer;

public interface ITickProvider
{
    Task DelayAsync(TimeSpan interval, CancellationToken cancellationToken);
}
