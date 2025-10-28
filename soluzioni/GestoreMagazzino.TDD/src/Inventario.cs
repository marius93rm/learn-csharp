using System.Collections.ObjectModel;

namespace GestoreMagazzino;

/// <summary>
/// Gestisce l'insieme di prodotti presenti a magazzino coordinando creazione, scorte e notifiche.
/// </summary>
public class Inventario
{
    /// <summary>
    /// Dizionario interno che permette ricerche veloci per codice prodotto.
    /// Utilizziamo un comparer case-insensitive per evitare duplicati che differiscono solo per maiuscole/minuscole.
    /// </summary>
    private readonly Dictionary<string, Prodotto> _prodotti =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Restituisce l'elenco dei prodotti attualmente gestiti dal magazzino.
    /// </summary>
    public IReadOnlyCollection<Prodotto> ElencoProdotti()
    {
        // Convertiamo la vista interna in una ReadOnlyCollection per proteggere l'incapsulamento.
        return new ReadOnlyCollection<Prodotto>(_prodotti.Values.ToList());
    }

    /// <summary>
    /// Aggiunge un nuovo prodotto all'inventario.
    /// </summary>
    /// <param name="codice">Codice univoco del prodotto.</param>
    /// <param name="descrizione">Descrizione leggibile del prodotto.</param>
    /// <exception cref="InvalidOperationException">Quando esiste già un prodotto con lo stesso codice.</exception>
    public void AggiungiProdotto(string codice, string descrizione)
    {
        // Creiamo il prodotto: il costruttore si occupa di validare i parametri.
        var nuovoProdotto = new Prodotto(codice, descrizione);

        // Controlliamo se il codice è già presente per evitare duplicati.
        if (_prodotti.ContainsKey(nuovoProdotto.Codice))
        {
            throw new InvalidOperationException($"Esiste già un prodotto con codice {nuovoProdotto.Codice}");
        }

        // Una volta validato, inseriamo il prodotto nel dizionario interno.
        _prodotti.Add(nuovoProdotto.Codice, nuovoProdotto);
    }

    /// <summary>
    /// Rimuove un prodotto identificato dal codice.
    /// </summary>
    /// <param name="codice">Codice del prodotto da rimuovere.</param>
    /// <exception cref="ArgumentException">Quando il codice non è valido.</exception>
    /// <exception cref="KeyNotFoundException">Quando il prodotto non esiste.</exception>
    public void RimuoviProdotto(string codice)
    {
        // Garantiamo che il codice sia stato fornito.
        if (string.IsNullOrWhiteSpace(codice))
        {
            throw new ArgumentException("Il codice prodotto è obbligatorio", nameof(codice));
        }

        // Se il codice non è presente nel dizionario, rendiamo esplicito l'errore.
        if (!_prodotti.Remove(codice))
        {
            throw new KeyNotFoundException($"Nessun prodotto trovato con codice {codice}");
        }
    }

    /// <summary>
    /// Aggiunge scorte a un prodotto già presente.
    /// </summary>
    /// <param name="codice">Codice del prodotto da aggiornare.</param>
    /// <param name="quantita">Quantità da aggiungere.</param>
    /// <exception cref="KeyNotFoundException">Se il prodotto non esiste.</exception>
    public void AggiungiScorta(string codice, int quantita)
    {
        // Recuperiamo il prodotto, fallendo immediatamente se il codice è errato.
        var prodotto = RecuperaProdotto(codice);

        // Delegando l'aggiornamento al prodotto, centralizziamo le regole di validazione.
        prodotto.IncrementaQuantita(quantita);
    }

    /// <summary>
    /// Rimuove scorte da un prodotto esistente e valuta eventuali notifiche.
    /// </summary>
    /// <param name="codice">Codice del prodotto da aggiornare.</param>
    /// <param name="quantitaDaRimuovere">Quantità di scorta da scalare.</param>
    /// <param name="sogliaNotifica">Soglia sotto la quale scatta la notifica.</param>
    /// <param name="notificatore">Componente incaricato di comunicare l'evento verso l'esterno.</param>
    /// <exception cref="ArgumentNullException">Se il notificatore non è fornito.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Se la soglia è negativa.</exception>
    public void RimuoviScorta(string codice, int quantitaDaRimuovere, int sogliaNotifica, INotificatoreMagazzino notificatore)
    {
        // Prima di tutto ci assicuriamo che qualcuno gestisca la notifica.
        if (notificatore is null)
        {
            throw new ArgumentNullException(nameof(notificatore));
        }

        // Una soglia negativa non avrebbe senso nel dominio.
        if (sogliaNotifica < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sogliaNotifica), "La soglia non può essere negativa");
        }

        // Recuperiamo il prodotto ed effettuiamo la rimozione della quantità.
        var prodotto = RecuperaProdotto(codice);
        var quantitaPrecedente = prodotto.Quantita;

        prodotto.DecrementaQuantita(quantitaDaRimuovere);

        // Se siamo passati da sopra soglia a sotto soglia, notifichiamo l'evento.
        if (quantitaPrecedente > sogliaNotifica && prodotto.Quantita <= sogliaNotifica)
        {
            notificatore.NotificaScorteInEsaurimento(prodotto, sogliaNotifica);
        }
    }

    /// <summary>
    /// Utility centralizzata per estrarre un prodotto gestendo gli errori in modo uniforme.
    /// </summary>
    private Prodotto RecuperaProdotto(string codice)
    {
        if (string.IsNullOrWhiteSpace(codice))
        {
            throw new ArgumentException("Il codice prodotto è obbligatorio", nameof(codice));
        }

        if (!_prodotti.TryGetValue(codice, out var prodotto))
        {
            throw new KeyNotFoundException($"Nessun prodotto trovato con codice {codice}");
        }

        return prodotto;
    }
}
