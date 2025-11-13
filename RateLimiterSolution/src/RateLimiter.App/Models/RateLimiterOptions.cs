using System;

namespace RateLimiter.App.Models;

/// <summary>
/// Rappresenta l'insieme delle opzioni di configurazione del rate limiter
/// per gli esercizi guidati del progetto.
/// </summary>
public sealed class RateLimiterOptions
{
    /// <summary>
    /// Inizializza le opzioni con valori di default ragionevoli.
    /// </summary>
    public RateLimiterOptions()
    {
        MaxConcurrentRequests = 3;
        MaxRequestsPerWindow = 10;
        WindowLength = TimeSpan.FromSeconds(1);
        SimulatedWorkDuration = TimeSpan.FromMilliseconds(400);
        GlobalTimeout = TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// Numero massimo di richieste che possono eseguire simultaneamente.
    /// </summary>
    public int MaxConcurrentRequests { get; set; }

    /// <summary>
    /// Numero massimo di richieste accettate all'interno della finestra temporale.
    /// </summary>
    public int MaxRequestsPerWindow { get; set; }

    /// <summary>
    /// Durata della finestra temporale utilizzata per contare le richieste.
    /// </summary>
    public TimeSpan WindowLength { get; set; }

    /// <summary>
    /// Durata del lavoro simulato eseguito da ogni richiesta.
    /// </summary>
    public TimeSpan SimulatedWorkDuration { get; set; }

    /// <summary>
    /// Timeout globale opzionale per tutte le richieste.
    /// </summary>
    public TimeSpan? GlobalTimeout { get; set; }
}
