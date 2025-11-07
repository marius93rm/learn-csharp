using System;
using System.Linq;

namespace LogGuidato.Step4;

/// <summary>
/// Entry point dello step 4: dimostra l'uso del logger generico.
/// </summary>
public static class Program
{
    public static void Main()
    {
        var logger = new Logger<EventoAccesso>();

        // Registriamo alcuni eventi di esempio. Ogni lambda riceve l'istanza appena creata
        // e la configura impostando le proprietà esposte dal tipo concreto.
        logger.RegistraEvento(evt =>
        {
            evt.Utente = "mrossi";
            evt.Messaggio = "Accesso riuscito dalla web app.";
            evt.IndirizzoIp = "10.1.2.3";
            evt.Categoria = "ACCESSO_OK";
        });

        logger.RegistraEvento(evt =>
        {
            evt.Utente = "mbianchi";
            evt.Messaggio = "Password errata fornita dall'utente.";
            evt.Categoria = "ACCESSO_FALLITO";
        });

        logger.RegistraEvento(evt =>
        {
            evt.Utente = "service-account";
            evt.Messaggio = "Token di servizio rigenerato automaticamente.";
            evt.Categoria = "MANUTENZIONE";
        });

        Console.WriteLine("== Tutti gli eventi registrati ==");
        foreach (var evento in logger.RecuperaEventi())
        {
            Console.WriteLine(evento);
        }

        Console.WriteLine("\n== Eventi filtrati (solo errori) ==");
        foreach (var evento in logger.RecuperaEventi(evt => evt.Categoria.Contains("FALLITO", StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine(evento);
        }

        Console.WriteLine("\nTotale eventi: " + logger.RecuperaEventi().Count());
    }
}
