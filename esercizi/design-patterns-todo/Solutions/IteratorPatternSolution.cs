using System.Collections;

namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del pattern Iterator con supporto a percorsi inversi.
/// </summary>
public static class IteratorPatternSolution
{
    public static void Run()
    {
        var lessons = new CourseCollection();
        lessons.Add("Introduzione");
        lessons.Add("Prototype");
        lessons.Add("Builder");

        Console.WriteLine("Enumerazione normale:");
        foreach (var lesson in lessons)
        {
            Console.WriteLine($"- {lesson}");
        }

        Console.WriteLine("\nEnumerazione inversa:");
        foreach (var lesson in lessons.GetReverseEnumerable())
        {
            Console.WriteLine($"- {lesson}");
        }
    }

    private sealed class CourseCollection : IEnumerable<string>
    {
        private readonly List<string> _items = new();

        public void Add(string lesson) => _items.Add(lesson);

        public IEnumerator<string> GetEnumerator() => new ForwardIterator(_items);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<string> GetReverseEnumerable() => new ReverseEnumerable(_items);
    }

    private sealed class ForwardIterator : IEnumerator<string>
    {
        private readonly IReadOnlyList<string> _items;
        private int _index = -1;

        public ForwardIterator(IReadOnlyList<string> items) => _items = items;

        public string Current => _items[_index];
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return _index < _items.Count;
        }

        public void Reset() => _index = -1;

        public void Dispose()
        {
            // In questo esempio non abbiamo risorse da liberare, ma il metodo viene mantenuto
            // per mostrare come gestire l'interfaccia completa di IEnumerator.
        }
    }

    private sealed class ReverseEnumerable : IEnumerable<string>
    {
        private readonly IReadOnlyList<string> _items;

        public ReverseEnumerable(IReadOnlyList<string> items) => _items = items;

        public IEnumerator<string> GetEnumerator() => new ReverseIterator(_items);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class ReverseIterator : IEnumerator<string>
    {
        private readonly IReadOnlyList<string> _items;
        private int _index;

        public ReverseIterator(IReadOnlyList<string> items)
        {
            _items = items;
            _index = items.Count;
        }

        public string Current => _items[_index];
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index--;
            return _index >= 0;
        }

        public void Reset() => _index = _items.Count;

        public void Dispose()
        {
        }
    }
}
