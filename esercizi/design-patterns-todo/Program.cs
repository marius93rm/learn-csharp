/*
 * Titolo: Esercizio TODO – Design Pattern GoF
 * Obiettivi formativi: comprendere i principali pattern GoF e implementarli in C#.
 * Istruzioni generali:
 *   - Studia ogni esempio e completa le sezioni contrassegnate da // TODO.
 *   - Compila ed esegui l'applicazione dopo ogni modifica per osservare il comportamento.
 *   - Modifica solo il codice necessario per soddisfare i TODO mantenendo lo stile didattico.
 */

using DesignPatternsTodo.Patterns;

namespace DesignPatternsTodo;

internal static class Program
{
    private static readonly (string Name, Action Demo)[] Exercises =
    {
        ("Prototype", PrototypePattern.Run),
        ("Builder", BuilderPattern.Run),
        ("Object Pool", ObjectPoolPattern.Run),
        ("Bridge", BridgePattern.Run),
        ("Composite", CompositePattern.Run),
        ("Flyweight", FlyweightPattern.Run),
        ("Proxy", ProxyPattern.Run),
        ("Chain of Responsibility", ChainOfResponsibilityPattern.Run),
        ("Iterator", IteratorPattern.Run),
        ("Mediator", MediatorPattern.Run),
        ("Memento", MementoPattern.Run),
        ("State", StatePattern.Run),
        ("Template Method", TemplateMethodPattern.Run),
        ("Visitor", VisitorPattern.Run)
    };

    private static void Main()
    {
        Console.WriteLine("=== Esercizio TODO – Design Pattern GoF ===\n");
        Console.WriteLine("Per ogni pattern osserva l'output iniziale, poi completa i TODO per arricchire il comportamento.\n");

        foreach (var (name, demo) in Exercises)
        {
            Console.WriteLine($"--- {name} Pattern ---");
            demo();
            Console.WriteLine();
        }

        Console.WriteLine("Fine dimostrazione. Ora completa i TODO!\n");
    }
}
