using System.Collections;

namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Iterator.
/// Completa i TODO per creare iterazioni personalizzate.
/// </summary>
public static class IteratorPattern
{
    public static void Run()
    {
        var lessons = new CourseCollection();
        lessons.Add("Introduzione");
        lessons.Add("Prototype");
        lessons.Add("Builder");

        foreach (var lesson in lessons)
        {
            Console.WriteLine($"Lezione: {lesson}");
        }

        Console.WriteLine("\nImplementa TODO per iterazioni alternative (es. ordine inverso o filtri).\n");
    }

    private sealed class CourseCollection : IEnumerable<string>
    {
        private readonly List<string> _items = new();

        public void Add(string lesson)
        {
            _items.Add(lesson);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return new ForwardIterator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // TODO: esponi qui un metodo per ottenere un iteratore personalizzato (es. ReverseIterator).
    }

    private sealed class ForwardIterator : IEnumerator<string>
    {
        private readonly IReadOnlyList<string> _items;
        private int _index = -1;

        public ForwardIterator(IReadOnlyList<string> items)
        {
            _items = items;
        }

        public string Current => _items[_index];
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return _index < _items.Count;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
            // TODO: valuta se è necessario liberare risorse qui (non in questo esempio ma utile per discuterne).
        }
    }
}
