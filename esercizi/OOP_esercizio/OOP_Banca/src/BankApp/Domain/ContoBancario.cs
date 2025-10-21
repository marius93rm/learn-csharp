namespace BankApp.Domain;

/// <summary>
/// Classe base astratta per i conti bancari.
/// </summary>
public abstract class ContoBancario
{
    /// <summary>
    /// TODO [M2]: valida che il numero di conto non sia nullo o vuoto.
    /// </summary>
    public string NumeroConto { get; }

    /// <summary>
    /// TODO [M2]: assicurati che il titolare non sia null.
    /// </summary>
    public Cliente Titolare { get; }

    /// <summary>
    /// TODO [M2]: il set deve essere accessibile solo dalle classi derivate.
    /// </summary>
    public decimal Saldo { get; protected set; }

    /// <summary>
    /// TODO [M5]: registra tutte le transazioni effettuate sul conto.
    /// </summary>
    public List<Transazione> Transazioni { get; } = new();

    protected ContoBancario(string numeroConto, Cliente titolare, decimal saldoIniziale = 0m)
    {
        // TODO [M2]: valida gli argomenti e inizializza correttamente il saldo.
        NumeroConto = numeroConto;
        Titolare = titolare;
        Saldo = saldoIniziale;
    }

    /// <summary>
    /// Deposita un importo sul conto.
    /// </summary>
    /// <param name="importo">L'importo da depositare.</param>
    public virtual void Deposita(decimal importo)
    {
        // TODO [M2]: valida l'importo (> 0) e aggiorna saldo e lista transazioni.
        throw new NotImplementedException("TODO M2: implementare Deposita.");
    }

    /// <summary>
    /// Preleva un importo dal conto.
    /// </summary>
    /// <param name="importo">L'importo da prelevare.</param>
    public abstract void Preleva(decimal importo);

    /// <summary>
    /// Stampa l'estratto conto corrente utilizzando Console.WriteLine.
    /// </summary>
    public virtual void StampaEstrattoConto()
    {
        // TODO [M5]: formatta l'estratto conto includendo tutte le transazioni ordinate cronologicamente.
        throw new NotImplementedException("TODO M5: implementare StampaEstrattoConto.");
    }

    protected void RegistraTransazione(Transazione transazione)
    {
        // TODO [M5]: aggiungi protezioni contro transazioni nulle.
        Transazioni.Add(transazione);
    }
}
