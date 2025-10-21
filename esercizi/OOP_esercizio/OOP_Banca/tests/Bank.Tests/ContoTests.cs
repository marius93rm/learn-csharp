using BankApp.Domain;
using Xunit;

namespace Bank.Tests;

public class ContoTests
{
    [Fact]
    public void Deposita_AumentaSaldo()
    {
        // TODO [Test]: implementa i test una volta completate le milestone principali.
        var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");
        var conto = new ContoCorrente("TEST-CC", cliente);

        Assert.Throws<NotImplementedException>(() => conto.Deposita(100m));
    }

    [Fact]
    public void Preleva_DiminuisceSaldo()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");
        var conto = new ContoCorrente("TEST-CC", cliente, saldoIniziale: 200m);

        Assert.Throws<NotImplementedException>(() => conto.Preleva(50m));
    }

    [Fact]
    public void Preleva_OltreLimiteSenzaFido_LanciaInvalidOperationException()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");
        var conto = new ContoCorrente("TEST-CC", cliente, saldoIniziale: 50m);

        // TODO [Test]: aggiorna questo test per verificare l'eccezione specifica una volta implementata la logica del fido.
        Assert.Throws<NotImplementedException>(() => conto.Preleva(200m));
    }

    [Fact]
    public void ApplicaInteressi_AumentaSaldo()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Giulia", "Bianchi");
        var conto = new ContoRisparmio("TEST-RS", cliente, saldoIniziale: 1000m, tassoInteresseAnnuale: 0.05m);

        Assert.Throws<NotImplementedException>(() => conto.ApplicaInteressi());
    }
}
