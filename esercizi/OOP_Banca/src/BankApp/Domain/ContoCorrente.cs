namespace BankApp.Domain;

/// <summary>
/// Conto corrente con possibilità di fido.
/// </summary>
public class ContoCorrente : ContoBancario
{
    /// <summary>
    /// TODO [M3]: valida che il fido non sia negativo.
    /// </summary>
    public decimal Fido { get; }

    public ContoCorrente(string numeroConto, Cliente titolare, decimal saldoIniziale = 0m, decimal fido = 0m)
        : base(numeroConto, titolare, saldoIniziale)
    {
        // TODO [M3]: applica validazioni sul fido opzionale.
        Fido = fido;
    }

    public override void Preleva(decimal importo)
    {
        // TODO [M3]: implementa la logica di prelievo considerando il fido.
        throw new NotImplementedException("TODO M3: implementare Preleva per ContoCorrente.");
    }
}
