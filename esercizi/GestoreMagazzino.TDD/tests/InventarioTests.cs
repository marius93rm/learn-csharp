using GestoreMagazzino;
using Moq;
using Xunit;

namespace GestoreMagazzino.Tests;

public class InventarioTests
{
    [Fact]
    public void InventarioIniziaVuoto()
    {
        // TODO 1.T1: Impostare il test iniziale che verifica l'inventario vuoto.
        // Suggerimento: seguire il ciclo Red-Green-Refactor partendo da un test fallente.
        throw new NotImplementedException();
    }

    [Fact]
    public void AggiuntaProdottoRendeVisibileIlProdotto()
    {
        // TODO 2.T1: Scrivere il test che fallisce finché AggiungiProdotto non è implementato.
        throw new NotImplementedException();
    }

    [Fact]
    public void NonPermetteDuplicatiERimuoveProdotti()
    {
        // TODO 3.T1: Modellare il comportamento atteso quando si inseriscono duplicati.
        // TODO 3.T2: Verificare le eccezioni appropriate durante la rimozione di un prodotto inesistente.
        throw new NotImplementedException();
    }

    [Fact]
    public void GestisceScorteEAggiornamentiDiQuantita()
    {
        // TODO 4.T1: Proteggere le regole sulle quantità (valori negativi, incremento).
        // TODO 4.T2: Testare gli scenari di refactoring, mantenendo i test verdi.
        throw new NotImplementedException();
    }

    [Fact]
    public void NotificaQuandoLaScortaScendeSottoLaSoglia()
    {
        // TODO 5.T1: Utilizzare Moq per verificare l'interazione con INotificatoreMagazzino.
        // TODO 5.T2: Simulare scenari con soglia e garantire che la notifica avvenga una sola volta.
        throw new NotImplementedException();
    }
}
