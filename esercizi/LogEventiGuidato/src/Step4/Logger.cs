using System;
using System.Collections.Generic;
using System.Linq;

namespace LogGuidato.Step4;

/// <summary>
/// Gestore generico degli eventi di log.
/// </summary>
public class Logger<T> where T : IEventoLoggabile, new()
{
    private readonly List<T> _eventi = new();

    public void RegistraEvento(Action<T> configurazione)
    {
        if (configurazione is null)
        {
            throw new ArgumentNullException(nameof(configurazione));
        }

        // TODO: creare un'istanza di T grazie al vincolo new().
        // var evento = new T();

        // TODO: applicare la configurazione e valorizzare le proprietà obbligatorie dell'evento.
        // configurazione(evento);

        // TODO: salvare l'evento nella lista interna.
        // _eventi.Add(evento);

        throw new NotImplementedException("Completa la registrazione dell'evento per proseguire.");
    }

    public IEnumerable<T> RecuperaEventi(Func<T, bool>? filtro = null)
    {
        // TODO: restituire la lista completa o filtrata degli eventi.
        return Enumerable.Empty<T>();
    }
}
