using System;

namespace LogGuidato.Step4;

/// <summary>
/// Evento di accesso riutilizzabile dal logger generico.
/// È configurabile tramite proprietà pubbliche che verranno impostate dalla lambda
/// passata a <see cref="Logger{T}.RegistraEvento"/>.
/// </summary>
public class EventoAccesso : IEventoLoggabile
{
    public EventoAccesso()
    {
        Timestamp = DateTime.UtcNow; // timestamp catturato automaticamente
    }

    /// <summary>
    /// Utente associato all'evento (opzionale: può essere valorizzato in seguito).
    /// </summary>
    public string? Utente { get; set; }

    /// <summary>
    /// Indirizzo IP dell'origine dell'accesso (campo facoltativo ma utile nei log reali).
    /// </summary>
    public string? IndirizzoIp { get; set; }

    public DateTime Timestamp { get; private set; }

    public string Messaggio { get; set; } = "Accesso completato.";

    /// <summary>
    /// Categoria obbligatoria richiesta dal contratto del logger. Di default "ACCESSO".
    /// </summary>
    public string Categoria { get; set; } = "ACCESSO";

    public override string ToString()
    {
        var utente = string.IsNullOrWhiteSpace(Utente) ? "<sconosciuto>" : Utente;
        var ip = string.IsNullOrWhiteSpace(IndirizzoIp) ? string.Empty : $" da IP {IndirizzoIp}";
        return $"[{Timestamp:O}] ({Categoria}) Utente {utente}{ip} -> {Messaggio}";
    }
}
