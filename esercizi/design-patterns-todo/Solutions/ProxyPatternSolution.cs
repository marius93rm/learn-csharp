namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione del pattern Proxy con verifica d'accesso e conteggio visualizzazioni.
/// </summary>
public static class ProxyPatternSolution
{
    public static void Run()
    {
        IImage image = new LazyImageProxy("grafico.png", currentUserRole: "editor");
        image.Display();
        image.Display();

        Console.WriteLine();

        IImage unauthorizedImage = new LazyImageProxy("segreto.png", currentUserRole: "ospite");
        unauthorizedImage.Display();
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
        private readonly string _currentUserRole;
        private readonly string _requiredRole = "editor";
        private HeavyImage? _realImage;
        private int _displayCount;

        public LazyImageProxy(string fileName, string currentUserRole)
        {
            _fileName = fileName;
            _currentUserRole = currentUserRole;
        }

        public void Display()
        {
            if (_currentUserRole != _requiredRole)
            {
                Console.WriteLine($"Accesso negato all'immagine '{_fileName}' per il ruolo '{_currentUserRole}'.");
                return;
            }

            if (_realImage is null)
            {
                Console.WriteLine("Proxy: autorizzazione verificata, creo l'immagine pesante.");
                _realImage = new HeavyImage(_fileName);
            }

            _displayCount++;
            Console.WriteLine($"Proxy: l'immagine è stata visualizzata {_displayCount} volte.");
            _realImage.Display();
        }
    }
}
