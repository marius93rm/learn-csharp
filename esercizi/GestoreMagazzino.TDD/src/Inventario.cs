namespace GestoreMagazzino;

public class Inventario
{
    // TODO 1.1: Inizializzare la struttura dati interna che conterrà i prodotti dell'inventario.

    public IReadOnlyCollection<Prodotto> ElencoProdotti()
    {
        // TODO 1.2: Restituire i prodotti attualmente presenti nel magazzino.
        throw new NotImplementedException();
    }

    public void AggiungiProdotto(string codice, string descrizione)
    {
        // TODO 2.1: Aggiungere un nuovo prodotto con codice e descrizione.
        // TODO 2.2: Assicurarsi che il prodotto aggiunto sia visibile in ElencoProdotti().
        throw new NotImplementedException();
    }

    public void RimuoviProdotto(string codice)
    {
        // TODO 3.1: Rimuovere il prodotto associato al codice indicato.
        // TODO 3.2: Impedire l'inserimento di prodotti con codice duplicato.
        // TODO 3.3: Definire e lanciare eccezioni significative quando l'operazione non è valida.
        throw new NotImplementedException();
    }

    public void AggiungiScorta(string codice, int quantita)
    {
        // TODO 4.1: Gestire la quantità disponibile per ciascun prodotto.
        // TODO 4.2: Incrementare la quantità di scorta in base al codice prodotto.
        throw new NotImplementedException();
    }

    public void RimuoviScorta(string codice, int quantitaDaRimuovere, int sogliaNotifica, INotificatoreMagazzino notificatore)
    {
        // TODO 4.3: Diminuire la quantità di scorta e validare i casi limite.
        // TODO 5.1: Attivare la notifica quando la quantità scende sotto la soglia indicata.
        // TODO 5.2: Collaborare con INotificatoreMagazzino rispettando il principio DIP.
        throw new NotImplementedException();
    }
}
