namespace BankApp.Domain;

/// <summary>
/// Conto di risparmio che applica interessi periodici.
/// </summary>
public class ContoRisparmio : ContoBancario
{
    /// <summary>
    /// TODO [M4]: valida che il tasso di interesse sia compreso in un intervallo ragionevole (> 0).
    /// </summary>
    public decimal TassoInteresseAnnuale { get; }

    public ContoRisparmio(string numeroConto, Cliente titolare, decimal saldoIniziale = 0m, decimal tassoInteresseAnnuale = 0m)
        : base(numeroConto, titolare, saldoIniziale)
    {
        // TODO [M4]: applica le validazioni necessarie sul tasso di interesse.
        TassoInteresseAnnuale = tassoInteresseAnnuale;
    }

    /// <summary>
    /// Applica gli interessi al conto.
    /// </summary>
    public void ApplicaInteressi()
    {
        // TODO [M4]: calcola gli interessi (es. mensili) e registra la relativa transazione "INTERESSI".
        throw new NotImplementedException("TODO M4: implementare ApplicaInteressi.");
    }

    public override void Preleva(decimal importo)
    {
        // TODO [M4]: implementa la logica di prelievo per il conto risparmio (senza fido).
        throw new NotImplementedException("TODO M4: implementare Preleva per ContoRisparmio.");
    }
}
