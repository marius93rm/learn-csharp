using System;

namespace LogGuidato.Step2;

/// <summary>
/// Classe con metodi decorati da individuare via reflection.
/// </summary>
public class ServizioOrdini
{
    [Loggable("Crea un report sintetico")] // esempio già completato
    public void GeneraReport()
    {
        Console.WriteLine("Report generato.");
    }

    // TODO: aggiungere un secondo metodo loggabile con attributo e log personalizzato.

    // TODO: aggiungere un metodo non loggabile per confrontare il comportamento.
}
