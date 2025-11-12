using System;
using System.IO;

namespace OsservatoreFileSystem;

class Program
{
    static void Main()
    {
        /* Milestone 1: Chiedi all’utente il percorso da monitorare */
        Console.WriteLine("Benvenuto in OsservatoreFileSystem!");
        Console.Write("Inserisci il percorso completo della cartella da monitorare: ");
        var path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Percorso non valido. Il programma terminerà.");
            return;
        }

        if (!Directory.Exists(path))
        {
            Console.WriteLine($"La cartella '{path}' non esiste. Creala prima di avviare il monitoraggio.");
            return;
        }

        /* Milestone 2: Crea un oggetto FileSystemWatcher e configuralo */
        using var watcher = new FileSystemWatcher(path)
        {
            IncludeSubdirectories = false,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.Size
        };

        /* Milestone 3: Gestisci eventi Created, Changed, Deleted, Renamed */
        watcher.Created += OnCreated;
        watcher.Changed += OnChanged;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;

        watcher.EnableRaisingEvents = true;

        Console.WriteLine();
        Console.WriteLine($"Monitoraggio avviato su: {path}");
        Console.WriteLine("Premi un tasto qualsiasi per terminare il programma...");

        /* Milestone 4: Mantieni il programma in attesa con Console.ReadKey */
        Console.ReadKey(intercept: true);

        Console.WriteLine();
        Console.WriteLine("Monitoraggio terminato. Arrivederci!");
    }

    private static void OnCreated(object sender, FileSystemEventArgs e)
    {
        PrintEvent("CREATO", e.Name);
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        // Alcuni editor generano eventi multipli, quindi filtriamo cambiamenti su directory
        if (Directory.Exists(e.FullPath))
        {
            return;
        }

        PrintEvent("MODIFICATO", e.Name);
    }

    private static void OnDeleted(object sender, FileSystemEventArgs e)
    {
        PrintEvent("ELIMINATO", e.Name);
    }

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[RINOMINATO] da {e.OldName} a {e.Name} alle {timestamp}");
    }

    private static void PrintEvent(string eventName, string? fileName)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[{eventName}] {fileName} alle {timestamp}");
    }
}
