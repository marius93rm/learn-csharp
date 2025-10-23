namespace BankApp.Domain;

/// <summary>
/// Conto corrente con possibilità di fido.
/// </summary>
public class ContoCorrente : ContoBancario
{
    /// <summary>
    /// Importo massimo che il conto può andare in rosso.
    /// </summary>
    public decimal Fido { get; }

    public ContoCorrente(string numeroConto, Cliente titolare, decimal saldoIniziale = 0m, decimal fido = 0m)
        : base(numeroConto, titolare, saldoIniziale)
    {
        if (fido < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(fido), "Il fido non può essere negativo.");
        }

        Fido = fido;
    }

    public override void Preleva(decimal importo)
    {
        if (importo <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(importo), "L'importo da prelevare deve essere positivo.");
        }

        // Calcola il saldo risultante dopo il prelievo.
        var saldoDisponibile = Saldo + Fido;
        if (importo > saldoDisponibile)
        {
            throw new InvalidOperationException("Fondi insufficienti considerando il fido disponibile.");
        }

        Saldo -= importo;
        RegistraTransazione(new Transazione(DateTime.Now, importo, "PRELIEVO", "Prelievo da conto corrente"));
    }
}
