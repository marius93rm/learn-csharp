namespace GestoreMagazzino;

/// <summary>
/// Rappresenta l'unità minima gestita dal magazzino.
/// Ogni prodotto è identificato da un codice univoco e da una descrizione leggibile.
/// La quantità è l'unico dato mutabile perché deve poter crescere o diminuire nel tempo.
/// </summary>
public class Prodotto
{
    /// <summary>
    /// Inizializza un nuovo prodotto impostandone le informazioni statiche.
    /// Notare come la quantità parta sempre da zero: sarà l'inventario a modificarla.
    /// </summary>
    /// <param name="codice">Identificativo univoco del prodotto.</param>
    /// <param name="descrizione">Descrizione a beneficio dell'utente.</param>
    /// <exception cref="ArgumentException">Generata quando i parametri sono vuoti o solo spazi.</exception>
    public Prodotto(string codice, string descrizione)
    {
        // Validiamo immediatamente il codice per garantire l'integrità del dominio.
        if (string.IsNullOrWhiteSpace(codice))
        {
            throw new ArgumentException("Il codice prodotto è obbligatorio", nameof(codice));
        }

        // Anche la descrizione deve essere presente: aiuta a comprendere cosa stiamo gestendo.
        if (string.IsNullOrWhiteSpace(descrizione))
        {
            throw new ArgumentException("La descrizione del prodotto è obbligatoria", nameof(descrizione));
        }

        Codice = codice;
        Descrizione = descrizione;
        Quantita = 0;
    }

    /// <summary>
    /// Codice univoco del prodotto. È immutabile dopo la costruzione per evitare inconsistenze.
    /// </summary>
    public string Codice { get; }

    /// <summary>
    /// Descrizione del prodotto. Manteniamo il set privato così da poterla cambiare con metodi dedicati in futuro.
    /// </summary>
    public string Descrizione { get; private set; }

    /// <summary>
    /// Quantità attualmente a magazzino. Può essere modificata solo attraverso i metodi controllati sottostanti.
    /// </summary>
    public int Quantita { get; private set; }

    /// <summary>
    /// Incrementa la quantità a magazzino.
    /// </summary>
    /// <param name="quantita">Numero di unità da aggiungere.</param>
    /// <exception cref="ArgumentOutOfRangeException">Quando si prova ad aggiungere un valore non positivo.</exception>
    public void IncrementaQuantita(int quantita)
    {
        // Prima regola: ha senso aggiungere solo quantità positive.
        if (quantita <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantita), "La quantità da aggiungere deve essere positiva");
        }

        // Una volta validati i dati, aggiorniamo lo stato.
        Quantita += quantita;
    }

    /// <summary>
    /// Riduce la quantità disponibile per il prodotto.
    /// </summary>
    /// <param name="quantita">Numero di unità da rimuovere.</param>
    /// <exception cref="ArgumentOutOfRangeException">Se la quantità richiesta non è positiva.</exception>
    /// <exception cref="InvalidOperationException">Se si richiede di rimuovere più scorte di quelle disponibili.</exception>
    public void DecrementaQuantita(int quantita)
    {
        // Anche qui controlliamo immediatamente l'input.
        if (quantita <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantita), "La quantità da rimuovere deve essere positiva");
        }

        // Non possiamo scendere sotto zero: proteggiamo quindi l'invariante.
        if (quantita > Quantita)
        {
            throw new InvalidOperationException("Impossibile rimuovere più scorte di quelle disponibili");
        }

        // Infine aggiorniamo lo stato interno del prodotto.
        Quantita -= quantita;
    }
}
