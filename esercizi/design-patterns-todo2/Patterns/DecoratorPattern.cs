/*
 * Pattern: Decorator
 * Obiettivi didattici:
 *   - Aggiungere responsabilità dinamicamente a un oggetto.
 *   - Comporre decoratori multipli mantenendo l'interfaccia originale.
 *   - Comprendere la differenza con l'ereditarietà tradizionale.
 * Istruzioni:
 *   - Completa i TODO per introdurre nuovi decoratori e configurazioni dinamiche.
 */

namespace DesignPatternsTodo2.Patterns;

public static class DecoratorPattern
{
    public static void Run()
    {
        INotification notification = new BasicNotification();
        notification = new EmailDecorator(notification, "studente@example.com");
        notification = new SmsDecorator(notification, "+391234567890");

        notification.Send("Benvenuto al corso!");

        // TODO: prova a creare una catena di decoratori diversa (es. SlackDecorator) e invoca Send.
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
        Console.WriteLine($"[Email] Invio a {_emailAddress}: {message}");
        // TODO: aggiungi validazione indirizzo email e gestione errori di invio.
    }
}

public sealed class SmsDecorator : NotificationDecorator
{
    private readonly string _phoneNumber;

    public SmsDecorator(INotification inner, string phoneNumber) : base(inner)
    {
        _phoneNumber = phoneNumber;
    }

    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"[SMS] Invio a {_phoneNumber}: {message}");
        // TODO: implementa un limite di lunghezza messaggio o un sistema di queueing.
    }
}

// TODO: aggiungi nuovi decoratori (es. PushNotificationDecorator) per estendere le funzionalità.
