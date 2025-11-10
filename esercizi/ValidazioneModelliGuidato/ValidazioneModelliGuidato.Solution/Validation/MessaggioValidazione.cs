namespace ValidazioneModelliGuidato.Solution.Validation;

/// <summary>
/// Livello logico associato a un messaggio di validazione.
/// </summary>
public enum LivelloMessaggio
{
    Informazione,
    Avviso,
    Errore
}

/// <summary>
/// Contiene il testo e il livello di severità di una singola occorrenza
/// prodotta durante la validazione.
/// </summary>
/// <param name="Livello">Severità del messaggio (Errore blocca la validazione).</param>
/// <param name="Testo">Descrizione destinata all'utente finale.</param>
public readonly record struct MessaggioValidazione(LivelloMessaggio Livello, string Testo)
{
    /// <summary>
    /// Fabbrica rapida per creare un messaggio informativo.
    /// </summary>
    public static MessaggioValidazione Info(string testo) => new(LivelloMessaggio.Informazione, testo);

    /// <summary>
    /// Fabbrica rapida per creare un messaggio di avviso.
    /// </summary>
    public static MessaggioValidazione Warning(string testo) => new(LivelloMessaggio.Avviso, testo);

    /// <summary>
    /// Fabbrica rapida per creare un messaggio di errore.
    /// </summary>
    public static MessaggioValidazione Error(string testo) => new(LivelloMessaggio.Errore, testo);
}
