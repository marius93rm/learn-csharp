using System;

namespace RateLimiter.App.Models;

/// <summary>
/// Rappresenta l'esito della lavorazione di una richiesta simulata attraverso il rate limiter.
/// </summary>
public sealed class RequestResult
{
    /// <summary>
    /// Identificatore della richiesta associata al risultato.
    /// </summary>
    public int RequestId { get; init; }

    /// <summary>
    /// Indica se la richiesta è stata accettata dal rate limiter.
    /// </summary>
    public bool Accepted { get; init; }

    /// <summary>
    /// Momento di avvio della lavorazione (per richieste accettate).
    /// </summary>
    public DateTime StartTime { get; init; }

    /// <summary>
    /// Momento di completamento della lavorazione (per richieste accettate).
    /// </summary>
    public DateTime EndTime { get; init; }

    /// <summary>
    /// Descrizione testuale dell'esito della richiesta.
    /// </summary>
    public string Outcome { get; init; } = string.Empty;
}
