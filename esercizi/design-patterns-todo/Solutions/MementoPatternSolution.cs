namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Gestione avanzata della cronologia con metadati, limite massimo e redo.
/// </summary>
public static class MementoPatternSolution
{
    public static void Run()
    {
        var editor = new TextEditor();
        var history = new EditorHistory(maxHistory: 5);

        editor.Type("Ciao");
        history.Push(editor.Save("Alice"));

        editor.Type(", mondo!");
        history.Push(editor.Save("Alice"));

        editor.Type(" Questo è un test.");
        history.Push(editor.Save("Alice"));

        editor.Restore(history.Undo());
        Console.WriteLine($"Undo -> {editor.Content}");

        editor.Restore(history.Redo());
        Console.WriteLine($"Redo -> {editor.Content}");
    }

    private sealed class TextEditor
    {
        public string Content { get; private set; } = string.Empty;

        public void Type(string text) => Content += text;

        public EditorMemento Save(string author)
        {
            Console.WriteLine("Stato salvato.");
            return new EditorMemento(Content, DateTimeOffset.UtcNow, author);
        }

        public void Restore(EditorMemento memento)
        {
            Content = memento.Content;
            Console.WriteLine($"Stato ripristinato ({memento.Timestamp:t} da {memento.Author}).");
        }
    }

    private sealed record EditorMemento(string Content, DateTimeOffset Timestamp, string Author);

    private sealed class EditorHistory
    {
        private readonly int _maxHistory;
        private readonly List<EditorMemento> _timeline = new();
        private int _position = -1;

        public EditorHistory(int maxHistory)
        {
            _maxHistory = Math.Max(1, maxHistory);
        }

        public void Push(EditorMemento memento)
        {
            if (_position < _timeline.Count - 1)
            {
                // Quando salviamo un nuovo stato dopo un undo eliminiamo il futuro.
                _timeline.RemoveRange(_position + 1, _timeline.Count - _position - 1);
            }

            _timeline.Add(memento);
            if (_timeline.Count > _maxHistory)
            {
                _timeline.RemoveAt(0);
            }

            _position = _timeline.Count - 1;
        }

        public EditorMemento Undo()
        {
            if (_timeline.Count == 0)
            {
                throw new InvalidOperationException("Cronologia vuota.");
            }

            if (_position <= 0)
            {
                Console.WriteLine("Raggiunto l'inizio della cronologia.");
                _position = 0;
                return _timeline[_position];
            }

            _position--;
            return _timeline[_position];
        }

        public EditorMemento Redo()
        {
            if (_timeline.Count == 0)
            {
                throw new InvalidOperationException("Cronologia vuota.");
            }

            if (_position >= _timeline.Count - 1)
            {
                Console.WriteLine("Nessun elemento successivo da ripristinare.");
                return _timeline[_position];
            }

            _position++;
            return _timeline[_position];
        }
    }
}
