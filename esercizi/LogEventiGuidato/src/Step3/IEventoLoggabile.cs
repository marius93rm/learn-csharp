using System;

namespace LogGuidato.Step3;

/// <summary>
/// Contratto che ogni evento loggabile deve rispettare.
/// </summary>
public interface IEventoLoggabile
{
    DateTime Timestamp { get; }

    string Messaggio { get; }

    // TODO: aggiungere una proprietà o metodo che identifichi il tipo di evento (es. string TipoEvento { get; }).
}
