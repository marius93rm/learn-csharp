using System.Collections.Generic;
using System.Linq;
using ValidazioneModelliGuidato.Solution.Models;
using ValidazioneModelliGuidato.Solution.Validation;

namespace ValidazioneModelliGuidato.Solution;

/// <summary>
/// Entry point della console application che dimostra il funzionamento del sistema di validazione.
/// </summary>
internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Validazione modelli guidato - Soluzione finale ==\n");
        Console.WriteLine("L'esempio mostra come aggiungere nuove regole senza modificare il validatore generico.\n");

        var utenti = new List<Utente>
        {
            new()
            {
                Nome = "Ada",
                Email = "ada.lovelace@example.com",
                Password = "Analytical",
                Ruoli = { "Admin", "Teacher" }
            },
            new()
            {
                Nome = "Alan",
                Email = "alan.turing@example.com",
                Password = "Enigma", // Meno di 8 caratteri -> errore sulla password.
                Ruoli = { "Researcher" }
            },
            new()
            {
                Nome = null, // Manca il nome.
                Email = "", // Vuoto -> errore obbligatorio.
                Password = "", // Vuoto -> doppio errore (obbligatorio e lunghezza minima).
                Ruoli = { } // Lista vuota -> errore min lunghezza.
            },
            new()
            {
                Nome = "Grace",
                Email = null, // Null -> errore obbligatorio con messaggio personalizzato.
                Password = "Cobol2023",
                Ruoli = { "Maintainer" }
            }
        };

        var validatore = new Validatore<Utente>();
        var risultati = validatore.ValidaTutti(utenti);

        foreach (var esito in risultati)
        {
            StampaEsito(esito);
        }
    }

    private static void StampaEsito(EsitoValidazione<Utente> esito)
    {
        Console.WriteLine($"Utente: {esito.Istanza} -> Valido: {esito.Valido}");

        foreach (var messaggio in esito.Messaggi)
        {
            var prefisso = messaggio.Livello switch
            {
                LivelloMessaggio.Errore => "   [Errore]",
                LivelloMessaggio.Avviso => "   [Avviso]",
                _ => "   [Info]"
            };

            Console.WriteLine($"{prefisso} {messaggio.Testo}");
        }

        if (!esito.Messaggi.Any())
        {
            Console.WriteLine("   Nessun messaggio: tutti i controlli superati.");
        }

        Console.WriteLine();
    }
}
