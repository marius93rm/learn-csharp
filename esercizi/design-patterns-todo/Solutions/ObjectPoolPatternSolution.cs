namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Implementazione risolta dell'Object Pool.
/// La soluzione introduce una preallocazione controllata, la validazione dei
/// rilasci e il ripristino dello stato dell'oggetto riutilizzato.
/// </summary>
public static class ObjectPoolPatternSolution
{
    public static void Run()
    {
        var pool = new BulletPool(() => new Bullet());

        var bulletA = pool.Acquire();
        bulletA.FireAt("Target A");
        pool.Release(bulletA);

        var bulletB = pool.Acquire();
        bulletB.FireAt("Target B");
        pool.Release(bulletB);
    }

    private sealed class BulletPool
    {
        private const int PreallocationSize = 3;
        private readonly Func<Bullet> _factory;
        private readonly Queue<Bullet> _available = new();
        private readonly HashSet<Bullet> _inUse = new();

        public BulletPool(Func<Bullet> factory)
        {
            _factory = factory;
            Preallocate();
        }

        public Bullet Acquire()
        {
            if (_available.Count == 0)
            {
                // Quando il pool è vuoto creiamo un piccolo lotto di oggetti extra.
                Preallocate();
            }

            var bullet = _available.Dequeue();
            _inUse.Add(bullet);
            return bullet;
        }

        public void Release(Bullet bullet)
        {
            if (!_inUse.Remove(bullet))
            {
                // Segnaliamo immediatamente l'uso scorretto per evitare che oggetti
                // sconosciuti entrino nel pool.
                throw new InvalidOperationException("Il proiettile non appartiene al pool corrente.");
            }

            bullet.Reset();
            _available.Enqueue(bullet);
        }

        private void Preallocate()
        {
            for (var i = 0; i < PreallocationSize; i++)
            {
                var bullet = _factory();
                bullet.Reset();
                _available.Enqueue(bullet);
            }
        }
    }

    private sealed class Bullet
    {
        private static int _nextId;

        public Bullet()
        {
            Id = ++_nextId;
        }

        public int Id { get; }
        public string? LastTarget { get; private set; }
        public DateTimeOffset? FiredAt { get; private set; }

        public void FireAt(string target)
        {
            LastTarget = target;
            FiredAt = DateTimeOffset.UtcNow;
            Console.WriteLine($"Bullet #{Id} -> {target}");
        }

        public void Reset()
        {
            // Puliamo tutte le informazioni specifiche dell'ultimo utilizzo
            // così il proiettile torna ad uno stato consistente.
            LastTarget = null;
            FiredAt = null;
        }
    }
}
