using System.Reflection;

namespace ValidazioneModelliGuidato.Solution.Validation;

/// <summary>
/// Contratto che ogni attributo di validazione deve rispettare.
/// L'obiettivo è mantenere il validatore aperto a nuove regole
/// senza dover modificare il suo codice.
/// </summary>
public interface IRegolaValidazione
{
    /// <summary>
    /// Esegue la logica di controllo sul valore della proprietà.
    /// </summary>
    /// <param name="valore">Valore della proprietà letto in modo dinamico tramite reflection.</param>
    /// <param name="proprieta">Metadati della proprietà in analisi (nome, tipo, attributi, ...).</param>
    /// <returns>
    /// Un messaggio di validazione quando la regola non è soddisfatta oppure
    /// <c>null</c> quando la regola è superata senza segnalazioni.
    /// </returns>
    MessaggioValidazione? Verifica(object? valore, PropertyInfo proprieta);
}
