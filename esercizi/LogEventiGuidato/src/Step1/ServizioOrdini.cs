using System;

namespace LogGuidato.Step1;

/// <summary>
/// Classe di esempio con metodi da decorare per il log.
/// </summary>
public class ServizioOrdini
{
    public void GeneraReport()
    {
        Console.WriteLine("Esecuzione metodo non loggato.");
    }

    // TODO: decorare il metodo con l'attributo [Loggable] per abilitarne il monitoraggio.
    public void ElaboraOrdine()
    {
        Console.WriteLine("Ordine elaborato.");
    }

    // TODO: aggiungere un secondo metodo loggabile (con parametri) che simuli un controllo stato.
}
