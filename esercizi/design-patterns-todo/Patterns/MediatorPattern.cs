namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Mediator.
/// Completa i TODO per coordinare nuovi componenti o notifiche.
/// </summary>
public static class MediatorPattern
{
    public static void Run()
    {
        var chat = new ChatRoomMediator();
        var alice = new ChatUser("Alice", chat);
        var bob = new ChatUser("Bob", chat);

        chat.Register(alice);
        chat.Register(bob);

        alice.Send("Ciao a tutti!");
        bob.Send("Benvenuta Alice!");

        Console.WriteLine("\nAggiungi funzionalità al mediatore completando i TODO.\n");
    }

    private interface IChatMediator
    {
        void Register(ChatUser user);
        void SendMessage(string message, ChatUser sender);
    }

    private sealed class ChatRoomMediator : IChatMediator
    {
        private readonly List<ChatUser> _participants = new();

        public void Register(ChatUser user)
        {
            _participants.Add(user);
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
        }

        // TODO: aggiungi qui logiche come messaggi privati, filtri o logging delle conversazioni.
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

        public void Receive(string message, ChatUser sender)
        {
            Console.WriteLine($"[{Name}] riceve da {sender.Name}: {message}");
            // TODO: aggiungi eventuali risposte automatiche o log personalizzati.
        }
    }
}
