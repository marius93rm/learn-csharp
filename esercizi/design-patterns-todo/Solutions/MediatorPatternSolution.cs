using System.Linq;

namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del pattern Mediator con supporto a messaggi privati e logging.
/// </summary>
public static class MediatorPatternSolution
{
    public static void Run()
    {
        var chat = new ChatRoomMediator();
        var alice = new ChatUser("Alice", chat);
        var bob = new ChatUser("Bob", chat);
        var carol = new ChatUser("Carol", chat);

        chat.Register(alice);
        chat.Register(bob);
        chat.Register(carol);

        alice.Send("Ciao a tutti!");
        bob.SendPrivate("Carol", "Ci vediamo alle 15?");
        carol.Send("Confermo, a dopo!");

        Console.WriteLine("\nLog conversazione:");
        foreach (var entry in chat.GetLog())
        {
            Console.WriteLine(entry);
        }
    }

    private interface IChatMediator
    {
        void Register(ChatUser user);
        void SendMessage(string message, ChatUser sender);
        void SendPrivateMessage(string message, ChatUser sender, string recipientName);
        IReadOnlyCollection<string> GetLog();
    }

    private sealed class ChatRoomMediator : IChatMediator
    {
        private readonly List<ChatUser> _participants = new();
        private readonly List<string> _log = new();

        public void Register(ChatUser user)
        {
            _participants.Add(user);
            _log.Add($"Sistema: {user.Name} si è unito alla chat.");
        }

        public void SendMessage(string message, ChatUser sender)
        {
            foreach (var participant in _participants)
            {
                if (participant != sender)
                {
                    participant.Receive(message, sender);
                }
            }

            _log.Add($"[{sender.Name}] {message}");
        }

        public void SendPrivateMessage(string message, ChatUser sender, string recipientName)
        {
            var recipient = _participants.FirstOrDefault(p => p.Name.Equals(recipientName, StringComparison.OrdinalIgnoreCase));
            if (recipient is null)
            {
                sender.Notify($"Impossibile trovare l'utente '{recipientName}'.");
                return;
            }

            recipient.ReceivePrivate(message, sender);
            _log.Add($"[Privato {sender.Name}->{recipient.Name}] {message}");
        }

        public IReadOnlyCollection<string> GetLog() => _log.AsReadOnly();
    }

    private sealed class ChatUser
    {
        private readonly IChatMediator _mediator;

        public ChatUser(string name, IChatMediator mediator)
        {
            Name = name;
            _mediator = mediator;
        }

        public string Name { get; }

        public void Send(string message)
        {
            Console.WriteLine($"[{Name}] invia: {message}");
            _mediator.SendMessage(message, this);
        }

        public void SendPrivate(string recipientName, string message)
        {
            Console.WriteLine($"[{Name}] invia un privato a {recipientName}: {message}");
            _mediator.SendPrivateMessage(message, this, recipientName);
        }

        public void Receive(string message, ChatUser sender)
        {
            Console.WriteLine($"[{Name}] riceve da {sender.Name}: {message}");
            if (message.Contains("ciao", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"[{Name}] risponde automaticamente: Piacere di sentirti!");
            }
        }

        public void ReceivePrivate(string message, ChatUser sender)
        {
            Console.WriteLine($"[{Name}] riceve un messaggio privato da {sender.Name}: {message}");
        }

        public void Notify(string message)
        {
            Console.WriteLine($"[{Name}] notifica: {message}");
        }
    }
}
