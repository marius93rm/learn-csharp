/*
 * Pattern: Singleton
 * Obiettivi didattici:
 *   - Comprendere come mantenere una singola istanza condivisa in tutta l'applicazione.
 *   - Gestire la thread-safety e l'inizializzazione pigra.
 *   - Esporre un punto di accesso globale con stato modificabile.
 * Istruzioni:
 *   - Completa i TODO per rendere l'implementazione robusta e flessibile.
 *   - Sperimenta aggiungendo proprietà e verificando che restino condivise.
 */

namespace DesignPatternsTodo2.Patterns;

public static class SingletonPattern
{
    public static void Run()
    {
        Console.WriteLine("Configurazione iniziale del gioco: " + GameConfiguration.Instance);

        GameConfiguration.Instance.Update("Hard", 2);
        Console.WriteLine("Configurazione aggiornata: " + GameConfiguration.Instance);

        Console.WriteLine("La stessa istanza viene riutilizzata: " + (ReferenceEquals(GameConfiguration.Instance, GameConfiguration.Instance) ? "Sì" : "No"));
    }
}

public sealed class GameConfiguration
{
    private static GameConfiguration? _instance;
    private static readonly object SyncRoot = new();

    private GameConfiguration()
    {
        Difficulty = "Normal";
        MaxPlayers = 4;
    }

    public static GameConfiguration Instance
    {
        get
        {
            // TODO: applica una strategia di inizializzazione pigra thread-safe (es. double-check locking o Lazy<T>).
            return _instance ??= new GameConfiguration();
        }
    }

    public string Difficulty { get; private set; }
    public int MaxPlayers { get; private set; }

    public void Update(string difficulty, int maxPlayers)
    {
        // TODO: aggiungi validazioni sui parametri (es. stringa non vuota, numero positivo).
        Difficulty = difficulty;
        MaxPlayers = maxPlayers;
        // TODO: inserisci un log o una notifica agli osservatori quando cambia la configurazione.
    }

    public override string ToString() => $"Difficulty={Difficulty}, MaxPlayers={MaxPlayers}";
}
