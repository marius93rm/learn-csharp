namespace LogGuidato.Step2;

/// <summary>
/// Entry point dello step 2: dimostra l'esecuzione dinamica tramite reflection.
/// </summary>
public static class Program
{
    public static void Main()
    {
        var servizio = new ServizioOrdini();
        ReflectionRunner.EseguiLoggable(servizio);
    }
}
