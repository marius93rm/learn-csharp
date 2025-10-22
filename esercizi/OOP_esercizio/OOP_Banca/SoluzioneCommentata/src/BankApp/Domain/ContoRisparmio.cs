namespace BankApp.Domain;

/// <summary>
/// Conto di risparmio che applica interessi periodici.
/// </summary>
public class ContoRisparmio : ContoBancario
{
    /// <summary>
    /// Tasso di interesse annuo espresso in forma decimale (es. 0.05 = 5%).
    /// </summary>
    public decimal TassoInteresseAnnuale { get; }

    public ContoRisparmio(string numeroConto, Cliente titolare, decimal saldoIniziale = 0m, decimal tassoInteresseAnnuale = 0m)
        : base(numeroConto, titolare, saldoIniziale)
    {
        if (tassoInteresseAnnuale <= 0m || tassoInteresseAnnuale > 1m)
        {
            throw new ArgumentOutOfRangeException(nameof(tassoInteresseAnnuale), "Il tasso deve essere compreso tra 0 e 1.");
        }

        TassoInteresseAnnuale = tassoInteresseAnnuale;
    }

    /// <summary>
    /// Applica gli interessi al conto.
    /// </summary>
    public void ApplicaInteressi()
    {
        if (Saldo <= 0m)
        {
            return; // Nessun interesse da applicare su saldo nullo o negativo.
        }

        // Calcoliamo gli interessi mensili partendo dal tasso annuo.
        var interessi = Saldo * (TassoInteresseAnnuale / 12m);
        if (interessi <= 0m)
        {
            return;
        }

        Saldo += interessi;
        RegistraTransazione(new Transazione(DateTime.Now, interessi, "INTERESSI", "Interessi maturati"));
    }

    public override void Preleva(decimal importo)
    {
        if (importo <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(importo), "L'importo da prelevare deve essere positivo.");
        }

        if (importo > Saldo)
        {
            throw new InvalidOperationException("Fondi insufficienti sul conto risparmio.");
        }

        Saldo -= importo;
        RegistraTransazione(new Transazione(DateTime.Now, importo, "PRELIEVO", "Prelievo da conto risparmio"));
    }
}
