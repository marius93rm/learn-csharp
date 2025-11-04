namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del pattern Composite con supporto a rimozione e conteggio.
/// </summary>
public static class CompositePatternSolution
{
    public static void Run()
    {
        INode root = new Folder("Corso Design Pattern");
        root.Add(new Document("Appunti.md"));
        root.Add(new Document("Diagramma.drawio"));

        var subFolder = new Folder("Esempi");
        subFolder.Add(new Document("Prototype.cs"));
        subFolder.Add(new Document("Builder.cs"));
        root.Add(subFolder);

        root.Display("-");
        Console.WriteLine($"Totale elementi: {root.Count()}");

        Console.WriteLine("\nRimuovo 'Builder.cs'...");
        root.Remove("Builder.cs");
        root.Display("-");
        Console.WriteLine($"Totale elementi dopo la rimozione: {root.Count()}");
    }

    private interface INode
    {
        string Name { get; }
        void Add(INode node);
        void Display(string prefix);
        bool Remove(string name);
        int Count();
    }

    private sealed class Document : INode
    {
        public Document(string name) => Name = name;

        public string Name { get; }

        public void Add(INode node)
        {
            Console.WriteLine($"Impossibile aggiungere '{node.Name}' a un documento singolo.");
        }

        public void Display(string prefix)
        {
            Console.WriteLine($"{prefix} Documento: {Name}");
        }

        public bool Remove(string name)
        {
            // Non potendo contenere altri nodi, la rimozione non ha effetto.
            return false;
        }

        public int Count() => 1;
    }

    private sealed class Folder : INode
    {
        private readonly List<INode> _children = new();

        public Folder(string name) => Name = name;

        public string Name { get; }

        public void Add(INode node) => _children.Add(node);

        public void Display(string prefix)
        {
            Console.WriteLine($"{prefix} Cartella: {Name}");
            foreach (var child in _children)
            {
                child.Display(prefix + "-");
            }
        }

        public bool Remove(string name)
        {
            var removed = _children.RemoveAll(node => node.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (removed > 0)
            {
                return true;
            }

            foreach (var child in _children)
            {
                if (child.Remove(name))
                {
                    return true;
                }
            }

            return false;
        }

        public int Count()
        {
            var total = 1; // includiamo la cartella stessa
            foreach (var child in _children)
            {
                total += child.Count();
            }

            return total;
        }
    }
}
