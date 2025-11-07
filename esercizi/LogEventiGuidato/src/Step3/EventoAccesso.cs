using System;

namespace LogGuidato.Step3;

/// <summary>
/// Implementazione di esempio dell'interfaccia <see cref="IEventoLoggabile"/>.
/// </summary>
public class EventoAccesso : IEventoLoggabile
{
    public EventoAccesso(string utente)
    {
        Utente = utente;
        Timestamp = DateTime.UtcNow;

        // TODO: valorizzare Messaggio e TipoEvento usando le informazioni disponibili.
    }

    public DateTime Timestamp { get; private set; }

    public string Messaggio { get; private set; } = "TODO: imposta un messaggio descrittivo per il log.";

    // TODO: sincronizzare questa proprietà con l'interfaccia una volta completata.
    public string TipoEvento { get; private set; } = "TODO: specifica il tipo di evento (es. ACCESSO).";

    public string Utente { get; }

    public override string ToString()
    {
        // TODO: restituire una stringa formattata con tutte le informazioni rilevanti.
        return $"Evento da completare per l'utente {Utente}";
    }
}
