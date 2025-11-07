namespace LogGuidato.Step1;

/// <summary>
/// Entry point dello step 1: esegue manualmente i metodi della classe di esempio.
/// Ogni invocazione è accompagnata da messaggi esplicativi così da distinguere
/// chiaramente i metodi loggati da quelli "normali".
/// </summary>
public static class Program
{
    public static void Main()
    {
        var servizio = new ServizioOrdini();

        Console.WriteLine("== Metodi non loggati ==");
        servizio.GeneraReport();

        Console.WriteLine("\n== Metodi loggati manualmente ==");

        // 1) Invocazione del primo metodo marcato con [Loggable].
        //    In questo step non c'è ancora automazione: ci limitiamo a richiamarlo a mano
        //    per verificare che la decorazione non influenzi l'esecuzione "normale".
        servizio.ElaboraOrdine();

        // 2) Invocazione del metodo con parametro. Passiamo un id simbolico
        //    per simulare la consultazione dello stato di un ordine.
        var idEsempio = 1284;
        servizio.ControllaStatoOrdine(idEsempio);

        Console.WriteLine("\nNota: nello step 2 useremo la reflection per intercettare automaticamente questi metodi.");
    }
}
