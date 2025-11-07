using System;

namespace LogGuidato.Step3;

/// <summary>
/// Contratto che ogni evento loggabile deve rispettare.
/// Lo manteniamo minimale ma sufficiente per descrivere un'operazione significativa.
/// </summary>
public interface IEventoLoggabile
{
    /// <summary>
    /// Istante in cui l'evento è stato generato (UTC per evitare ambiguità di fuso orario).
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// Messaggio descrittivo mostrato nel log.
    /// </summary>
    string Messaggio { get; }

    /// <summary>
    /// Categoria o "tipo" dell'evento (es. ACCESSO, ERRORE, AUDIT).
    /// </summary>
    string Categoria { get; }
}
