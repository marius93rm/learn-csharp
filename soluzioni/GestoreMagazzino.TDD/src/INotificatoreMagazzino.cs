namespace GestoreMagazzino;

/// <summary>
/// Contratto minimo che consente all'inventario di notificare eventi critici senza conoscere i dettagli dell'infrastruttura.
/// </summary>
public interface INotificatoreMagazzino
{
    /// <summary>
    /// Metodo invocato quando la quantità di un prodotto scende sotto la soglia richiesta.
    /// </summary>
    /// <param name="prodotto">Riferimento al prodotto interessato dall'evento.</param>
    /// <param name="soglia">Valore di soglia che ha fatto scattare la notifica.</param>
    void NotificaScorteInEsaurimento(Prodotto prodotto, int soglia);
}
