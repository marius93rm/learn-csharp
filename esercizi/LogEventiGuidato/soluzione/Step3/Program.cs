using System;

namespace LogGuidato.Step3;

/// <summary>
/// Entry point dello step 3: crea un evento e lo stampa.
/// </summary>
public static class Program
{
    public static void Main()
    {
        // 1) Creiamo un evento per un utente di test.
        var eventoSuccesso = new EventoAccesso("mrossi");

        // 2) Creiamo anche un evento di errore per mostrare il messaggio alternativo.
        var eventoFallito = new EventoAccesso("mbianchi", accessoConsentito: false);

        Console.WriteLine("== Eventi di accesso generati ==");
        Console.WriteLine(eventoSuccesso);
        Console.WriteLine(eventoFallito);

        Console.WriteLine("\nSuggerimento: questi oggetti verranno consumati dallo Step4 con un logger generico.");
    }
}
