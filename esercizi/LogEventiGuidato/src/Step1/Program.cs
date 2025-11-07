namespace LogGuidato.Step1;

/// <summary>
/// Entry point dello step 1: esegue manualmente i metodi della classe di esempio.
/// </summary>
public static class Program
{
    public static void Main()
    {
        var servizio = new ServizioOrdini();

        servizio.GeneraReport();

        // TODO: invocare qui i metodi decorati con [Loggable] per verificare il comportamento.
        // Suggerimento: prova a stampare un messaggio personalizzato per evidenziare i metodi tracciati.
    }
}
