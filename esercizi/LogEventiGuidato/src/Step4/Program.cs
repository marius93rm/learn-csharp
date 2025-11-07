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

        // TODO: registrare almeno due eventi configurando utente, messaggio e categoria.
        // Esempio: logger.RegistraEvento(evt => { ... });

        // TODO: recuperare gli eventi e stamparli, prevedendo un filtro opzionale (es. per utente).
        Console.WriteLine("Implementare le registrazioni e le stampe degli eventi.");
    }
}
