namespace LogGuidato.Step2;

/// <summary>
/// Entry point dello step 2: dimostra l'esecuzione dinamica tramite reflection.
/// </summary>
public static class Program
{
    public static void Main()
    {
        var servizio = new ServizioOrdini();

        // L'helper analizzerà il tipo, individuerà i metodi marcati con [Loggable]
        // e li eseguirà nell'ordine stabilito. Tutto ciò avviene senza conoscere
        // a priori i nomi dei metodi, sfruttando esclusivamente la reflection.
        ReflectionRunner.EseguiLoggable(servizio);
    }
}
