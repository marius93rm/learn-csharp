using System;

namespace LogGuidato.Step1;

/// <summary>
/// Classe di esempio con metodi decorati per il log.
/// Ogni metodo mostra un caso d'uso diverso per l'attributo <see cref="LoggableAttribute"/>.
/// </summary>
public class ServizioOrdini
{
    /// <summary>
    /// Metodo "normale": non è marcato come loggabile e quindi non verrà intercettato
    /// nelle fasi successive dell'esercizio. Serve per confrontare l'output.
    /// </summary>
    public void GeneraReport()
    {
        Console.WriteLine("[ServizioOrdini] Esecuzione metodo non loggato.");
    }

    /// <summary>
    /// Primo metodo decorato: l'attributo segnala che l'operazione va monitorata.
    /// La descrizione aiuta a capire cosa sta avvenendo quando leggeremo l'attributo.
    /// </summary>
    [Loggable("Elaborazione standard di un ordine")] // decorazione richiesta dal TODO originale
    public void ElaboraOrdine()
    {
        Console.WriteLine("[ServizioOrdini] Ordine elaborato con successo.");
    }

    /// <summary>
    /// Secondo metodo loggabile con parametri: simuliamo un controllo dello stato
    /// di un ordine specifico. Il parametro verrà passato manualmente da Program.cs.
    /// </summary>
    /// <param name="idOrdine">Identificativo dell'ordine da monitorare.</param>
    [Loggable("Verifica lo stato corrente dell'ordine fornito")]
    public void ControllaStatoOrdine(int idOrdine)
    {
        Console.WriteLine($"[ServizioOrdini] Stato dell'ordine {idOrdine}: IN_TRANSITO.");
    }
}
