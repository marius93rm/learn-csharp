using BankApp.Domain;
using Xunit;

namespace Bank.Tests;

public class ContoTests
{
    [Fact]
    public void Deposita_AumentaSaldo()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");
        var conto = new ContoCorrente("TEST-CC", cliente);

        conto.Deposita(100m);

        Assert.Equal(100m, conto.Saldo);
        Assert.Single(conto.Transazioni);
        Assert.Equal("DEPOSITO", conto.Transazioni[0].Tipo);
    }

    [Fact]
    public void Preleva_DiminuisceSaldo()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");
        var conto = new ContoCorrente("TEST-CC", cliente, saldoIniziale: 200m);

        conto.Preleva(50m);

        Assert.Equal(150m, conto.Saldo);
        Assert.Single(conto.Transazioni);
        Assert.Equal("PRELIEVO", conto.Transazioni[0].Tipo);
    }

    [Fact]
    public void Preleva_OltreLimiteSenzaFido_LanciaInvalidOperationException()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Mario", "Rossi");
        var conto = new ContoCorrente("TEST-CC", cliente, saldoIniziale: 50m);

        Assert.Throws<InvalidOperationException>(() => conto.Preleva(200m));
    }

    [Fact]
    public void ApplicaInteressi_AumentaSaldo()
    {
        var cliente = new Cliente(Guid.NewGuid(), "Giulia", "Bianchi");
        var conto = new ContoRisparmio("TEST-RS", cliente, saldoIniziale: 1000m, tassoInteresseAnnuale: 0.12m);

        conto.ApplicaInteressi();

        var saldoAtteso = 1000m + (1000m * (0.12m / 12m));
        Assert.Equal(decimal.Round(saldoAtteso, 2), decimal.Round(conto.Saldo, 2));
        Assert.Single(conto.Transazioni);
        Assert.Equal("INTERESSI", conto.Transazioni[0].Tipo);
    }
}
