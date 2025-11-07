using System;
using System.Collections.Generic;
using System.Linq;

namespace LogGuidato.Step4;

/// <summary>
/// Gestore generico degli eventi di log.
/// La classe è vincolata a tipi che implementano <see cref="IEventoLoggabile"/> e
/// dispongono di un costruttore pubblico senza parametri (grazie a <c>new()</c>).
/// </summary>
public class Logger<T> where T : IEventoLoggabile, new()
{
    private readonly List<T> _eventi = new();

    /// <summary>
    /// Crea e registra un nuovo evento applicando la configurazione fornita.
    /// </summary>
    /// <param name="configurazione">Lambda che popola le proprietà dell'evento.</param>
    public void RegistraEvento(Action<T> configurazione)
    {
        if (configurazione is null)
        {
            throw new ArgumentNullException(nameof(configurazione));
        }

        // 1) Creiamo una nuova istanza di T sfruttando il vincolo new().
        var evento = new T();

        // 2) Applichiamo la configurazione esterna (tipicamente una lambda inline).
        configurazione(evento);

        // 3) Validazione minima: assicuriamoci che il messaggio non sia vuoto.
        if (string.IsNullOrWhiteSpace(evento.Messaggio))
        {
            throw new InvalidOperationException("Il messaggio dell'evento non può essere vuoto.");
        }

        // 4) Salviamo l'evento nella lista interna.
        _eventi.Add(evento);
    }

    /// <summary>
    /// Restituisce gli eventi registrati. Se viene fornito un filtro, applica la condizione.
    /// </summary>
    /// <param name="filtro">Predicate opzionale da usare per filtrare gli eventi.</param>
    public IEnumerable<T> RecuperaEventi(Func<T, bool>? filtro = null)
    {
        if (filtro is null)
        {
            // Copiamo in un nuovo array per evitare modifiche esterne alla lista interna.
            return _eventi.ToArray();
        }

        return _eventi.Where(filtro).ToArray();
    }
}
