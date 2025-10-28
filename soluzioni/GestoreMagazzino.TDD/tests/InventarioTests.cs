using GestoreMagazzino;
using Moq;
using Xunit;

namespace GestoreMagazzino.Tests;

/// <summary>
/// I test replicano il percorso TDD indicato nell'esercizio originale.
/// Ogni scenario è costruito seguendo l'ordine delle milestone per documentare l'evoluzione del design.
/// </summary>
public class InventarioTests
{
    [Fact]
    public void InventarioIniziaVuoto()
    {
        // Arrange: creiamo un inventario appena istanziato.
        var inventario = new Inventario();

        // Act: recuperiamo l'elenco dei prodotti.
        var prodotti = inventario.ElencoProdotti();

        // Assert: confermiamo che non sia presente alcun elemento.
        Assert.Empty(prodotti);
    }

    [Fact]
    public void AggiuntaProdottoRendeVisibileIlProdotto()
    {
        var inventario = new Inventario();

        // Act: aggiungiamo un singolo prodotto.
        inventario.AggiungiProdotto("SKU-01", "Mouse ottico");

        // Assert: il prodotto compare nell'elenco e le sue informazioni sono coerenti.
        var prodotto = Assert.Single(inventario.ElencoProdotti());
        Assert.Equal("SKU-01", prodotto.Codice);
        Assert.Equal("Mouse ottico", prodotto.Descrizione);
        Assert.Equal(0, prodotto.Quantita);
    }

    [Fact]
    public void NonPermetteDuplicatiERimuoveProdotti()
    {
        var inventario = new Inventario();
        inventario.AggiungiProdotto("SKU-01", "Mouse ottico");

        // Verifichiamo che un duplicato provochi un'eccezione significativa.
        Assert.Throws<InvalidOperationException>(() => inventario.AggiungiProdotto("SKU-01", "Mouse ottico"));

        // Confermiamo che il prodotto possa essere rimosso correttamente.
        inventario.RimuoviProdotto("SKU-01");
        Assert.Empty(inventario.ElencoProdotti());

        // E che la rimozione di un codice sconosciuto comunichi l'errore.
        Assert.Throws<KeyNotFoundException>(() => inventario.RimuoviProdotto("SKU-01"));
    }

    [Fact]
    public void GestisceScorteEAggiornamentiDiQuantita()
    {
        var inventario = new Inventario();
        inventario.AggiungiProdotto("SKU-01", "Mouse ottico");

        // L'aggiunta di scorte positive aumenta la quantità.
        inventario.AggiungiScorta("SKU-01", 5);
        Assert.Equal(5, inventario.ElencoProdotti().Single().Quantita);

        // I valori non positivi vengono bloccati dal dominio del prodotto.
        Assert.Throws<ArgumentOutOfRangeException>(() => inventario.AggiungiScorta("SKU-01", 0));

        // La rimozione di scorte segue le stesse regole di validazione.
        var notificatore = Mock.Of<INotificatoreMagazzino>();
        inventario.RimuoviScorta("SKU-01", 2, 1, notificatore);
        Assert.Equal(3, inventario.ElencoProdotti().Single().Quantita);

        // Non è possibile rimuovere più scorte di quelle disponibili.
        Assert.Throws<InvalidOperationException>(() => inventario.RimuoviScorta("SKU-01", 5, 1, notificatore));
    }

    [Fact]
    public void NotificaQuandoLaScortaScendeSottoLaSoglia()
    {
        var inventario = new Inventario();
        inventario.AggiungiProdotto("SKU-01", "Mouse ottico");
        inventario.AggiungiScorta("SKU-01", 5);

        // Prepariamo un mock per verificare le interazioni con il notificatore.
        var mockNotificatore = new Mock<INotificatoreMagazzino>();

        // Con una rimozione che porta la scorta sotto soglia ci aspettiamo una singola notifica.
        inventario.RimuoviScorta("SKU-01", 4, 2, mockNotificatore.Object);

        mockNotificatore.Verify(
            n => n.NotificaScorteInEsaurimento(
                It.Is<Prodotto>(p => p.Codice == "SKU-01" && p.Quantita == 1),
                2),
            Times.Once);

        // Se restiamo sotto soglia, ulteriori rimozioni non fanno scattare una nuova notifica perché la soglia era già stata superata in precedenza.
        inventario.RimuoviScorta("SKU-01", 1, 2, mockNotificatore.Object);

        mockNotificatore.Verify(
            n => n.NotificaScorteInEsaurimento(It.IsAny<Prodotto>(), It.IsAny<int>()),
            Times.Once);
    }
}
