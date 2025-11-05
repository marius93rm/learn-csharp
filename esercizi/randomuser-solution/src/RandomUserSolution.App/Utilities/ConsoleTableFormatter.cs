// ---------------------------------------------------------------------------------------------------------------------
// ConsoleTableFormatter.cs
// ---------------------------------------------------------------------------------------------------------------------
// La classe si occupa di generare una tabella testuale a partire da una collezione di RandomUser.
// Anche se la logica è relativamente semplice, tenerla separata evita che Program.cs diventi troppo lungo.
// ---------------------------------------------------------------------------------------------------------------------

using RandomUserSolution.App.Models;

namespace RandomUserSolution.App.Utilities;

/// <summary>
/// Stampa una tabella con le informazioni principali degli utenti.
/// </summary>
public static class ConsoleTableFormatter
{
    public static void PrintPeople(IReadOnlyList<RandomUser> people, AppOptions options)
    {
        if (people.Count == 0)
        {
            Console.WriteLine("Nessun utente da mostrare.");
            return;
        }

        // Header che riassume i filtri applicati.
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"RandomUser — {people.Count} risultati");
        Console.ResetColor();
        Console.WriteLine($"Filtri: nazionalità={(options.NationalityFilter ?? "*"),-4} | genere={(options.GenderFilter ?? "*"),-6} | seed={(options.Seed ?? "*")}");
        Console.WriteLine(new string('-', 90));

        // Calcoliamo la larghezza delle colonne per allineare il testo.
        var nameWidth = Math.Max("Nome".Length, people.Max(p => p.FullName.Length));
        var emailWidth = Math.Max("Email".Length, people.Max(p => p.Email.Length));
        var phoneWidth = Math.Max("Telefono".Length, people.Max(p => p.Phone.Length));

        // Intestazione.
        Console.WriteLine($"{"Nome".PadRight(nameWidth)} | {"Email".PadRight(emailWidth)} | {"Naz".PadRight(3)} | {"Genere".PadRight(6)} | {"Telefono".PadRight(phoneWidth)}");
        Console.WriteLine(new string('-', nameWidth + emailWidth + phoneWidth + 15));

        foreach (var person in people)
        {
            Console.WriteLine($"{person.FullName.PadRight(nameWidth)} | {person.Email.PadRight(emailWidth)} | {person.Nationality.PadRight(3)} | {person.Gender.PadRight(6)} | {person.Phone.PadRight(phoneWidth)}");
        }

        Console.WriteLine(new string('-', nameWidth + emailWidth + phoneWidth + 15));
        Console.WriteLine("Consiglio: usa --seed per risultati ripetibili durante le prove.");
    }
}
