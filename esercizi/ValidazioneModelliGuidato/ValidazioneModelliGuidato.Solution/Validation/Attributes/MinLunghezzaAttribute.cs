using System;
using System.Collections;
using System.Reflection;
using ValidazioneModelliGuidato.Solution.Validation;

namespace ValidazioneModelliGuidato.Solution.Validation.Attributes;

/// <summary>
/// Verifica che il valore associato alla proprietà abbia una lunghezza minima.
/// Supporta sia stringhe sia raccolte (<see cref="ICollection"/>).
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MinLunghezzaAttribute : Attribute, IRegolaValidazione
{
    public MinLunghezzaAttribute(int lunghezzaMinima)
    {
        if (lunghezzaMinima < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lunghezzaMinima), "La lunghezza minima deve essere positiva.");
        }

        LunghezzaMinima = lunghezzaMinima;
    }

    /// <summary>
    /// Valore minimo consentito.
    /// </summary>
    public int LunghezzaMinima { get; }

    /// <inheritdoc />
    public MessaggioValidazione? Verifica(object? valore, PropertyInfo proprieta)
    {
        if (valore is null)
        {
            // Il controllo sull'obbligatorietà è delegato all'attributo specifico.
            return null;
        }

        // Caso stringa: usiamo la lunghezza del testo.
        if (valore is string testo)
        {
            return testo.Length < LunghezzaMinima
                ? MessaggioValidazione.Error($"La proprietà {proprieta.Name} deve contenere almeno {LunghezzaMinima} caratteri.")
                : null;
        }

        // Caso raccolta: verifichiamo il numero di elementi.
        if (valore is ICollection collection)
        {
            return collection.Count < LunghezzaMinima
                ? MessaggioValidazione.Error($"La raccolta {proprieta.Name} deve contenere almeno {LunghezzaMinima} elementi.")
                : null;
        }

        // Per altri tipi restituiamo un avviso non bloccante così il chiamante può correggere l'uso dell'attributo.
        return MessaggioValidazione.Warning(
            $"[Avviso] {nameof(MinLunghezzaAttribute)} è pensato per stringhe o raccolte. Tipo trovato: {proprieta.PropertyType.Name}."
        );
    }
}
