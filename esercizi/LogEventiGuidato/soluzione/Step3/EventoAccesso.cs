using System;

namespace LogGuidato.Step3;

/// <summary>
/// Implementazione di esempio dell'interfaccia <see cref="IEventoLoggabile"/>.
/// Rappresenta l'accesso (riuscito o fallito) di un utente al sistema.
/// </summary>
public class EventoAccesso : IEventoLoggabile
{
    public EventoAccesso(string utente, bool accessoConsentito = true)
    {
        if (string.IsNullOrWhiteSpace(utente))
        {
            throw new ArgumentException("Il nome utente è obbligatorio.", nameof(utente));
        }

        Utente = utente;
        AccessoConsentito = accessoConsentito;

        // Il timestamp viene catturato al momento della creazione per congelare l'istante del log.
        Timestamp = DateTime.UtcNow;

        // Categoria e messaggio sono derivati dalle informazioni ricevute.
        Categoria = "ACCESSO";
        Messaggio = accessoConsentito
            ? $"Accesso consentito per l'utente {Utente}."
            : $"Tentativo di accesso fallito per l'utente {Utente}.";
    }

    public DateTime Timestamp { get; }

    public string Messaggio { get; }

    public string Categoria { get; }

    /// <summary>
    /// Utente coinvolto nell'evento (informazione aggiuntiva rispetto al contratto).
    /// </summary>
    public string Utente { get; }

    /// <summary>
    /// Indica se l'accesso è andato a buon fine; utile per filtrare i log.
    /// </summary>
    public bool AccessoConsentito { get; }

    public override string ToString()
    {
        var esito = AccessoConsentito ? "SUCCESSO" : "FALLITO";
        return $"[{Timestamp:O}] ({Categoria}-{esito}) Utente: {Utente} -> {Messaggio}";
    }
}
