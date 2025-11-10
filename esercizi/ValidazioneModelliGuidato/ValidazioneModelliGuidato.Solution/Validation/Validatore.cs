namespace ValidazioneModelliGuidato.Solution.Validation;

/// <summary>
/// Validatore generico riutilizzabile per ogni modello che implementa <see cref="IValidabile"/>.
/// </summary>
public class Validatore<T> where T : IValidabile
{
    /// <summary>
    /// Esegue la validazione su una singola istanza e restituisce i messaggi prodotti.
    /// </summary>
    public EsitoValidazione<T> Valida(T istanza)
    {
        if (istanza is null)
        {
            throw new ArgumentNullException(nameof(istanza));
        }

        var valido = istanza.Valida(out var messaggi);
        return new EsitoValidazione<T>(istanza, valido, messaggi);
    }

    /// <summary>
    /// Esegue la validazione su tutte le istanze passate, sfruttando la lazy evaluation per evitare
    /// allocazioni non necessarie quando l'utente itera parzialmente la sequenza.
    /// </summary>
    public IEnumerable<EsitoValidazione<T>> ValidaTutti(IEnumerable<T> istanze)
    {
        if (istanze is null)
        {
            throw new ArgumentNullException(nameof(istanze));
        }

        foreach (var elemento in istanze)
        {
            yield return Valida(elemento);
        }
    }
}

/// <summary>
/// Rappresenta l'esito della validazione di un singolo elemento.
/// </summary>
public sealed record EsitoValidazione<T>(T Istanza, bool Valido, IReadOnlyCollection<MessaggioValidazione> Messaggi);
