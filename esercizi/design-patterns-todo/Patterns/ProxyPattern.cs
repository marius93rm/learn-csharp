namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Proxy.
/// Completa i TODO per simulare controllo accessi o caching.
/// </summary>
public static class ProxyPattern
{
    public static void Run()
    {
        IImage image = new LazyImageProxy("grafico.png");

        Console.WriteLine("Prima visualizzazione (carica l'immagine reale):");
        image.Display();

        Console.WriteLine("Seconda visualizzazione (riutilizza il caricamento):");
        image.Display();

        Console.WriteLine("\nEspandi il proxy implementando logiche aggiuntive nei TODO.\n");
    }

    private interface IImage
    {
        void Display();
    }

    private sealed class HeavyImage : IImage
    {
        public HeavyImage(string fileName)
        {
            FileName = fileName;
            Console.WriteLine($"Caricamento immagine '{FileName}' da disco...");
        }

        public string FileName { get; }

        public void Display()
        {
            Console.WriteLine($"Mostro l'immagine {FileName}.");
        }
    }

    private sealed class LazyImageProxy : IImage
    {
        private readonly string _fileName;
        private HeavyImage? _realImage;

        public LazyImageProxy(string fileName)
        {
            _fileName = fileName;
        }

        public void Display()
        {
            if (_realImage is null)
            {
                // TODO: inserisci qui eventuali controlli di autorizzazione o logging prima di creare l'oggetto reale.
                _realImage = new HeavyImage(_fileName);
            }

            // TODO: aggiungi qui eventuali controlli dopo l'accesso (es. conteggio accessi, caching avanzato).
            _realImage.Display();
        }
    }
}
