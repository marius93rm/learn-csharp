/*
 * Titolo: Esercizio TODO – Design Pattern GoF (Seconda Parte)
 * Obiettivi formativi: comprendere e implementare pattern creazionali, strutturali e comportamentali in C# moderno.
 * Istruzioni generali:
 *   - Esamina ciascun file nel namespace Patterns e completa i segnaposto contrassegnati da // TODO.
 *   - Compila ed esegui il progetto dopo ogni modifica per osservare il comportamento dei pattern.
 *   - Mantieni uno stile didattico, aggiungendo log o commenti utili quando necessario.
 */

using DesignPatternsTodo2.Patterns;

namespace DesignPatternsTodo2;

internal static class Program
{
    private static readonly (string Name, Action Demo)[] Exercises =
    {
        ("Singleton", SingletonPattern.Run),
        ("Factory Method", FactoryMethodPattern.Run),
        ("Abstract Factory", AbstractFactoryPattern.Run),
        ("Command", CommandPattern.Run),
        ("Interpreter", InterpreterPattern.Run),
        ("Strategy", StrategyPattern.Run),
        ("Decorator", DecoratorPattern.Run),
        ("Facade", FacadePattern.Run),
        ("Adapter", AdapterPattern.Run)
    };

    private static void Main()
    {
        Console.WriteLine("=== Esercizio TODO – Design Pattern GoF (Seconda Parte) ===\n");
        Console.WriteLine("Osserva l'output iniziale, poi completa i TODO per arricchire ciascun pattern.\n");

        foreach (var (name, demo) in Exercises)
        {
            Console.WriteLine($"--- {name} Pattern ---");
            demo();
            Console.WriteLine();
        }

        Console.WriteLine("Fine demo. È il tuo turno di completare i TODO!\n");
    }
}
