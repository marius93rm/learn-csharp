using System;

namespace RateLimiter.App.Models;

/// <summary>
/// Descrive una richiesta simulata che verrà processata dal rate limiter.
/// </summary>
public sealed class SimulatedRequest
{
    /// <summary>
    /// Identificatore incrementale della richiesta.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Timestamp di creazione della richiesta simulata.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
