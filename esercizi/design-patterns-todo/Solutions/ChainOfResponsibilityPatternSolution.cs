namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Catena di supporto con handler aggiuntivo e decisione finale del manager.
/// </summary>
public static class ChainOfResponsibilityPatternSolution
{
    public static void Run()
    {
        var supportChain = new LevelOneSupportHandler()
            .SetNext(new HardwareSupportHandler())
            .SetNext(new LevelTwoSupportHandler())
            .SetNext(new ManagerSupportHandler());

        foreach (var request in new[] { "password", "stampante", "rete", "contratto" })
        {
            Console.WriteLine($"Richiesta: {request}");
            supportChain.Handle(request);
            Console.WriteLine();
        }
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
            if (!Process(issue))
            {
                if (_next is not null)
                {
                    _next.Handle(issue);
                }
                else
                {
                    Console.WriteLine("Nessuno ha gestito la richiesta.");
                }
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

            if (issue.Contains("email", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Livello 1: assistenza configurazione email completata.");
                return true;
            }

            return false;
        }
    }

    private sealed class HardwareSupportHandler : SupportHandler
    {
        protected override bool Process(string issue)
        {
            if (issue.Contains("stampante", StringComparison.OrdinalIgnoreCase) || issue.Contains("hardware", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Hardware team: problema hardware diagnosticato e risolto.");
                return true;
            }

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
            Console.WriteLine("Il manager assegna un tecnico senior e chiude il ticket.");
            return true;
        }
    }
}
