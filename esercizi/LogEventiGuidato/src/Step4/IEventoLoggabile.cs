using System;

namespace LogGuidato.Step4;

/// <summary>
/// Contratto minimo richiesto dal logger generico.
/// </summary>
public interface IEventoLoggabile
{
    DateTime Timestamp { get; }

    string Messaggio { get; }

    // TODO: aggiungere altre informazioni obbligatorie, come un identificativo di categoria.
}
