namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Chain of Responsibility.
/// Completa i TODO per gestire nuovi tipi di richieste o per modificare la catena.
/// </summary>
public static class ChainOfResponsibilityPattern
{
    public static void Run()
    {
        var supportChain = new LevelOneSupportHandler()
            .SetNext(new LevelTwoSupportHandler())
            .SetNext(new ManagerSupportHandler());

        foreach (var request in new[] { "password", "rete", "contratto" })
        {
            Console.WriteLine($"Richiesta: {request}");
            supportChain.Handle(request);
            Console.WriteLine();
        }

        Console.WriteLine("\nAggiungi nuovi handler o modifica la catena completando i TODO.\n");
    }

    private abstract class SupportHandler
    {
        private SupportHandler? _next;

        public SupportHandler SetNext(SupportHandler next)
        {
            _next = next;
            return next;
        }

        public void Handle(string issue)
        {
            if (!Process(issue) && _next is not null)
            {
                _next.Handle(issue);
            }
            else if (_next is null)
            {
                Console.WriteLine("Nessuno ha gestito la richiesta.");
            }
        }

        protected abstract bool Process(string issue);
    }

    private sealed class LevelOneSupportHandler : SupportHandler
    {
        protected override bool Process(string issue)
        {
            if (issue.Contains("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Livello 1: reset password eseguito.");
                return true;
            }

            // TODO: gestisci qui altre richieste semplici che vuoi assegnare al livello 1.
            return false;
        }
    }

    private sealed class LevelTwoSupportHandler : SupportHandler
    {
        protected override bool Process(string issue)
        {
            if (issue.Contains("rete", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Livello 2: problema di rete diagnosticato.");
                return true;
            }

            return false;
        }
    }

    private sealed class ManagerSupportHandler : SupportHandler
    {
        protected override bool Process(string issue)
        {
            Console.WriteLine($"Manager informato per la richiesta: {issue}.");
            // TODO: decidi se il manager deve realmente risolvere o delegare ulteriormente.
            return true;
        }
    }
}
