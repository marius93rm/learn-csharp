using System;

namespace LogGuidato.Step4;

/// <summary>
/// Contratto minimo richiesto dal logger generico.
/// Gli eventi devono almeno fornire un timestamp, un messaggio e una categoria.
/// </summary>
public interface IEventoLoggabile
{
    DateTime Timestamp { get; }

    string Messaggio { get; }

    string Categoria { get; }
}
