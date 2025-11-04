using System;
using System.Threading;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del pattern Singleton con inizializzazione thread-safe e notifiche.
/// </summary>
public static class SingletonPatternSolution
{
    public static void Run()
    {
        GameConfiguration.ConfigurationChanged += config => Console.WriteLine($"Evento: configurazione aggiornata -> {config}");

        Console.WriteLine("Configurazione iniziale del gioco: " + GameConfiguration.Instance);

        GameConfiguration.Instance.Update("Hard", 2);
        Console.WriteLine("Configurazione aggiornata: " + GameConfiguration.Instance);

        Console.WriteLine("La stessa istanza viene riutilizzata: " + (ReferenceEquals(GameConfiguration.Instance, GameConfiguration.Instance) ? "Sì" : "No"));
    }
}

public sealed class GameConfiguration
{
    private static readonly Lazy<GameConfiguration> LazyInstance = new(() => new GameConfiguration(), LazyThreadSafetyMode.ExecutionAndPublication);

    private GameConfiguration()
    {
        Difficulty = "Normal";
        MaxPlayers = 4;
    }

    public static event Action<GameConfiguration>? ConfigurationChanged;

    public static GameConfiguration Instance => LazyInstance.Value;

    public string Difficulty { get; private set; }
    public int MaxPlayers { get; private set; }

    public void Update(string difficulty, int maxPlayers)
    {
        if (string.IsNullOrWhiteSpace(difficulty))
        {
            throw new ArgumentException("La difficoltà non può essere vuota.", nameof(difficulty));
        }

        if (maxPlayers <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxPlayers), "Il numero di giocatori deve essere positivo.");
        }

        Difficulty = difficulty;
        MaxPlayers = maxPlayers;
        Console.WriteLine($"Configurazione aggiornata manualmente: {this}");
        ConfigurationChanged?.Invoke(this);
    }

    public override string ToString() => $"Difficulty={Difficulty}, MaxPlayers={MaxPlayers}";
}
