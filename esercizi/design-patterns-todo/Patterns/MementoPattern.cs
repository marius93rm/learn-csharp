namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Memento.
/// Completa i TODO per gestire cronologie e ripristini multipli.
/// </summary>
public static class MementoPattern
{
    public static void Run()
    {
        var editor = new TextEditor();
        var history = new EditorHistory();

        editor.Type("Ciao");
        history.Push(editor.Save());

        editor.Type(", mondo!");
        Console.WriteLine($"Testo corrente: {editor.Content}");

        editor.Restore(history.Pop());
        Console.WriteLine($"Ripristinato: {editor.Content}");

        Console.WriteLine("\nCompleta i TODO per una gestione della cronologia più completa.\n");
    }

    private sealed class TextEditor
    {
        public string Content { get; private set; } = string.Empty;

        public void Type(string text)
        {
            Content += text;
        }

        public EditorMemento Save()
        {
            Console.WriteLine("Stato salvato.");
            return new EditorMemento(Content);
        }

        public void Restore(EditorMemento memento)
        {
            Content = memento.Content;
            Console.WriteLine("Stato ripristinato.");
        }
    }

    private sealed record EditorMemento(string Content)
    {
        // TODO: aggiungi meta-dati (timestamp, autore) per arricchire il memento.
    }

    private sealed class EditorHistory
    {
        private readonly Stack<EditorMemento> _history = new();

        public void Push(EditorMemento memento)
        {
            _history.Push(memento);
        }

        public EditorMemento Pop()
        {
            return _history.Pop();
        }

        // TODO: gestisci qui scenari come limite massimo o navigazione avanti/indietro.
    }
}
