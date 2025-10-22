namespace Pomodoro.App.Notify;

public class BeepNotifier : INotifier
{
    public void Notify(string message)
    {
        Console.Beep();
        Console.WriteLine(message);
    }
}
