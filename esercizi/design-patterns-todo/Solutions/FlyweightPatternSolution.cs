namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del pattern Flyweight con gestione esplicita della cache e dati intrinseci arricchiti.
/// </summary>
public static class FlyweightPatternSolution
{
    public static void Run()
    {
        var factory = new TreeFactory();
        factory.PlantTree(0, 0, "Quercia", "Verde scuro", "textures/oak.png", 12);
        factory.PlantTree(10, 5, "Quercia", "Verde scuro", "textures/oak.png", 12);
        factory.PlantTree(3, 7, "Pino", "Verde", "textures/pine.png", 9);

        Console.WriteLine($"Tipi unici creati: {factory.CreatedTypesCount}");
        factory.ClearCache();
        Console.WriteLine($"Cache svuotata, tipi ancora presenti: {factory.CreatedTypesCount}");
    }

    private sealed class TreeFactory
    {
        private readonly Dictionary<string, TreeType> _types = new();

        public int CreatedTypesCount => _types.Count;

        public Tree PlantTree(int x, int y, string name, string color, string texturePath, int averageHeight)
        {
            var key = $"{name}-{color}-{texturePath}-{averageHeight}";
            if (!_types.TryGetValue(key, out var type))
            {
                type = new TreeType(name, color, texturePath, averageHeight);
                _types[key] = type;
            }

            return new Tree(x, y, type);
        }

        public void ClearCache()
        {
            // Metodo di manutenzione: permette di rilasciare la memoria quando i flyweight
            // non servono più oppure di applicare politiche di scadenza.
            _types.Clear();
        }
    }

    private sealed record Tree(int X, int Y, TreeType Type)
    {
        public void Display()
        {
            Console.WriteLine($"Albero di tipo '{Type.Name}' al punto ({X},{Y}) con colore {Type.Color} (texture {Type.TexturePath}, altezza media {Type.AverageHeight}m).");
        }
    }

    private sealed record TreeType(string Name, string Color, string TexturePath, int AverageHeight);
}
