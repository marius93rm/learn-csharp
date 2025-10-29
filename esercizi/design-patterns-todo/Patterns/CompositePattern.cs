namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Composite.
/// Completa i TODO per gestire scenari più complessi nella gerarchia.
/// </summary>
public static class CompositePattern
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
        Console.WriteLine("\nCompleta i TODO per aggiungere, ad esempio, conteggio elementi o rimozione nodi.\n");
    }

    private interface INode
    {
        string Name { get; }
        void Add(INode node);
        void Display(string prefix);
        // TODO: valuta l'aggiunta di metodi come Remove o Count per arricchire il composite.
    }

    private sealed class Document : INode
    {
        public Document(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public void Add(INode node)
        {
            Console.WriteLine($"Impossibile aggiungere '{node.Name}' a un documento singolo.");
        }

        public void Display(string prefix)
        {
            Console.WriteLine($"{prefix} Documento: {Name}");
        }
    }

    private sealed class Folder : INode
    {
        private readonly List<INode> _children = new();

        public Folder(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public void Add(INode node)
        {
            _children.Add(node);
        }

        public void Display(string prefix)
        {
            Console.WriteLine($"{prefix} Cartella: {Name}");
            foreach (var child in _children)
            {
                child.Display(prefix + "-");
            }
        }

        // TODO: implementa qui le funzionalità aggiuntive scelte (es. Remove, conteggio ricorsivo, ricerca).
    }
}
