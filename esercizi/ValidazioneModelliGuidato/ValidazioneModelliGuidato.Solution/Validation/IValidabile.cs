namespace ValidazioneModelliGuidato.Solution.Validation;

/// <summary>
/// Interfaccia che i modelli devono implementare per essere gestiti dal validatore generico.
/// </summary>
public interface IValidabile
{
    /// <summary>
    /// Esegue le regole specifiche del modello restituendo i messaggi raccolti.
    /// </summary>
    /// <param name="messaggi">Risultato della validazione (errori, avvisi, informazioni).</param>
    /// <returns><c>true</c> se il modello è valido, altrimenti <c>false</c>.</returns>
    bool Valida(out List<MessaggioValidazione> messaggi);
}
