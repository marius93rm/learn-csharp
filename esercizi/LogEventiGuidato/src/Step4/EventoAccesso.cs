using System;

namespace LogGuidato.Step4;

/// <summary>
/// Evento di accesso riutilizzabile dal logger generico.
/// </summary>
public class EventoAccesso : IEventoLoggabile
{
    public EventoAccesso()
    {
        Timestamp = DateTime.UtcNow;
    }

    public string? Utente { get; set; }

    public DateTime Timestamp { get; private set; }

    public string Messaggio { get; set; } = "TODO: definire un messaggio di default significativo.";

    // TODO: aggiungere proprietà obbligatorie richieste dal contratto (es. Categoria o Gravità).

    public override string ToString()
    {
        // TODO: restituire un formato leggibile che includa timestamp, utente e messaggio.
        return $"EventoAccesso incompleto per l'utente {Utente ?? "<sconosciuto>"}.";
    }
}
