using System.Linq;

namespace BankApp.Domain;

/// <summary>
/// Classe base astratta per i conti bancari.
/// </summary>
public abstract class ContoBancario
{
    /// <summary>
    /// Numero univoco del conto.
    /// </summary>
    public string NumeroConto { get; }

    /// <summary>
    /// Cliente intestatario del conto.
    /// </summary>
    public Cliente Titolare { get; }

    /// <summary>
    /// Saldo corrente del conto (modificabile solo dalle classi derivate).
    /// </summary>
    public decimal Saldo { get; protected set; }

    /// <summary>
    /// Storico delle transazioni effettuate.
    /// </summary>
    public List<Transazione> Transazioni { get; } = new();

    protected ContoBancario(string numeroConto, Cliente titolare, decimal saldoIniziale = 0m)
    {
        // Valida il numero di conto.
        if (string.IsNullOrWhiteSpace(numeroConto))
        {
            throw new ArgumentException("Il numero di conto è obbligatorio.", nameof(numeroConto));
        }

        // Il titolare non può essere nullo.
        Titolare = titolare ?? throw new ArgumentNullException(nameof(titolare));

        // Il saldo iniziale non può essere negativo (nessun conto nasce in rosso senza fido esplicito).
        if (saldoIniziale < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(saldoIniziale), "Il saldo iniziale non può essere negativo.");
        }

        NumeroConto = numeroConto.Trim();
        Saldo = saldoIniziale;
    }

    /// <summary>
    /// Deposita un importo sul conto.
    /// </summary>
    /// <param name="importo">L'importo da depositare.</param>
    public virtual void Deposita(decimal importo)
    {
        if (importo <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(importo), "L'importo da depositare deve essere positivo.");
        }

        Saldo += importo;
        RegistraTransazione(new Transazione(DateTime.Now, importo, "DEPOSITO", "Deposito eseguito"));
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
        Console.WriteLine($"=== ESTRATTO CONTO ({NumeroConto}) — {Titolare} ===");

        if (Transazioni.Count == 0)
        {
            Console.WriteLine("Nessuna transazione registrata.\n");
            return;
        }

        var transazioniOrdinate = Transazioni.OrderBy(t => t.Data).ToList();
        decimal CalcolaDelta(Transazione t) => t.Tipo switch
        {
            "DEPOSITO" or "INTERESSI" => t.Importo,
            "PRELIEVO" => -t.Importo,
            _ => 0m
        };

        var saldoIniziale = Saldo - transazioniOrdinate.Sum(CalcolaDelta);
        decimal saldoProgressivo = saldoIniziale;

        foreach (var transazione in transazioniOrdinate)
        {
            // Calcola il nuovo saldo progressivo in base al tipo di movimento.
            saldoProgressivo += CalcolaDelta(transazione);

            // Stampa una riga formattata con data, tipo, importo e saldo.
            var importoFormattato = (transazione.Tipo == "PRELIEVO" ? -transazione.Importo : transazione.Importo)
                .ToString("+#,##0.00;-#,##0.00");
            Console.WriteLine($"{transazione.Data:yyyy-MM-dd HH:mm}  {transazione.Tipo,-10} {importoFormattato,8}   Saldo: {saldoProgressivo,8:0.00}");
        }

        Console.WriteLine();
    }

    protected void RegistraTransazione(Transazione transazione)
    {
        if (transazione is null)
        {
            throw new ArgumentNullException(nameof(transazione));
        }

        Transazioni.Add(transazione);
    }
}
