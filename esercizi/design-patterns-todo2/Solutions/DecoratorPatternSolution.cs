namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del pattern Decorator con validazioni e nuovi canali di notifica.
/// </summary>
public static class DecoratorPatternSolution
{
    public static void Run()
    {
        INotification notification = new BasicNotification();
        notification = new EmailDecorator(notification, "studente@example.com");
        notification = new PushNotificationDecorator(notification, "DesignPatternsApp");
        notification = new SmsDecorator(notification, "+391234567890");

        notification.Send("Benvenuto al corso!");
    }
}

public interface INotification
{
    void Send(string message);
}

public sealed class BasicNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"[Base] Notifica generica: {message}");
    }
}

public abstract class NotificationDecorator : INotification
{
    protected NotificationDecorator(INotification inner) => Inner = inner;

    protected INotification Inner { get; }

    public virtual void Send(string message)
    {
        Inner.Send(message);
    }
}

public sealed class EmailDecorator : NotificationDecorator
{
    private readonly string _emailAddress;

    public EmailDecorator(INotification inner, string emailAddress) : base(inner)
    {
        _emailAddress = emailAddress;
    }

    public override void Send(string message)
    {
        base.Send(message);
        if (!IsValidEmail(_emailAddress))
        {
            Console.WriteLine($"[Email] Indirizzo non valido: {_emailAddress}");
            return;
        }

        Console.WriteLine($"[Email] Invio a {_emailAddress}: {message}");
    }

    private static bool IsValidEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && email.Contains('@');
    }
}

public sealed class SmsDecorator : NotificationDecorator
{
    private readonly string _phoneNumber;
    private const int MaxLength = 120;

    public SmsDecorator(INotification inner, string phoneNumber) : base(inner)
    {
        _phoneNumber = phoneNumber;
    }

    public override void Send(string message)
    {
        base.Send(message);
        var truncated = message.Length > MaxLength ? message[..MaxLength] + "..." : message;
        Console.WriteLine($"[SMS] Invio a {_phoneNumber}: {truncated}");
    }
}

public sealed class PushNotificationDecorator : NotificationDecorator
{
    private readonly string _channel;

    public PushNotificationDecorator(INotification inner, string channel) : base(inner)
    {
        _channel = channel;
    }

    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"[Push] Canale {_channel}: {message}");
    }
}
