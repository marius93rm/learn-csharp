namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Object Pool.
/// Completa i TODO per gestire casi di utilizzo avanzati del pool.
/// </summary>
public static class ObjectPoolPattern
{
    public static void Run()
    {
        var pool = new BulletPool(() => new Bullet());

        var first = pool.Acquire();
        first.FireAt("Target A");

        pool.Release(first);

        var second = pool.Acquire();
        second.FireAt("Target B");

        Console.WriteLine("Osserva che lo stesso oggetto viene riutilizzato. Aggiungi logica extra seguendo i TODO.\n");
    }

    private sealed class BulletPool
    {
        private readonly Func<Bullet> _factory;
        private readonly Queue<Bullet> _available = new();
        private readonly HashSet<Bullet> _inUse = new();

        public BulletPool(Func<Bullet> factory)
        {
            _factory = factory;
        }

        public Bullet Acquire()
        {
            if (!_available.TryDequeue(out var bullet))
            {
                bullet = _factory();
                // TODO: valuta la possibilità di preallocare più proiettili qui (es. creando n elementi alla volta).
            }

            _inUse.Add(bullet);
            return bullet;
        }

        public void Release(Bullet bullet)
        {
            if (_inUse.Remove(bullet))
            {
                bullet.Reset();
                _available.Enqueue(bullet);
            }
            else
            {
                Console.WriteLine("Attenzione: il proiettile rilasciato non proveniva dal pool.");
                // TODO: gestisci questo caso (eccezione? log dettagliato?) in base alle esigenze dell'esercizio.
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

        public void FireAt(string target)
        {
            Console.WriteLine($"Bullet #{Id} -> {target}");
        }

        public void Reset()
        {
            // TODO: ripristina gli eventuali dati del proiettile (es. coordinate, velocità) quando li aggiungerai.
        }
    }
}
