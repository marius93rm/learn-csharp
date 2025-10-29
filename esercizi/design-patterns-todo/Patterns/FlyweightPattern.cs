namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Flyweight.
/// Completa i TODO per ottimizzare la gestione degli oggetti condivisi.
/// </summary>
public static class FlyweightPattern
{
    public static void Run()
    {
        var factory = new TreeFactory();

        var park = new List<Tree>
        {
            factory.PlantTree(0, 0, "Quercia", "Verde scuro"),
            factory.PlantTree(10, 5, "Quercia", "Verde scuro"),
            factory.PlantTree(3, 7, "Pino", "Verde");
        };

        foreach (var tree in park)
        {
            tree.Display();
        }

        Console.WriteLine($"Tipi unici creati: {factory.CreatedTypesCount}");
        Console.WriteLine("\nCompleta i TODO per aggiungere, ad esempio, caching avanzato o reset del factory.\n");
    }

    private sealed class TreeFactory
    {
        private readonly Dictionary<string, TreeType> _types = new();

        public int CreatedTypesCount => _types.Count;

        public Tree PlantTree(int x, int y, string name, string color)
        {
            var key = $"{name}-{color}";
            if (!_types.TryGetValue(key, out var type))
            {
                type = new TreeType(name, color);
                _types[key] = type;
            }

            return new Tree(x, y, type);
        }

        // TODO: aggiungi un metodo per svuotare o limitare la cache dei flyweight.
    }

    private sealed record Tree(int X, int Y, TreeType Type)
    {
        public void Display()
        {
            Console.WriteLine($"Albero di tipo '{Type.Name}' al punto ({X},{Y}) con colore {Type.Color}.");
        }
    }

    private sealed record TreeType(string Name, string Color)
    {
        // TODO: aggiungi qui eventuali dati intrinseci extra (texture, dimensioni) per osservare il beneficio della condivisione.
    }
}
