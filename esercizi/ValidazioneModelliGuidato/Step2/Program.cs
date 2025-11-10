// Step 2: Interfaccia IValidabile – In questo step introduciamo un contratto comune
// per tutti i modelli validabili e sfruttiamo la reflection per leggere gli attributi
// [Obbligatorio]. Completa i TODO per eseguire un controllo reale sui dati.

using System.Reflection;

namespace ValidazioneModelliGuidato.Step2;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ObbligatorioAttribute : Attribute
{
}

public interface IValidabile
{
    bool Valida(out List<string> errori);
}

public class Utente : IValidabile
{
    [Obbligatorio]
    public string? Nome { get; set; }

    [Obbligatorio]
    public string? Email { get; set; }

    public string? Note { get; set; }

    public bool Valida(out List<string> errori)
    {
        errori = new List<string>();

        foreach (var proprieta in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var valore = proprieta.GetValue(this);
            var haObbligatorio = Attribute.IsDefined(proprieta, typeof(ObbligatorioAttribute));

            if (!haObbligatorio)
            {
                continue;
            }

            // TODO: Gestisci il caso stringa vs. tipo reference generico.
            // Suggerimento: tratta stringhe vuote come errore, non solo valori null.
            if (valore is null)
            {
                errori.Add($"La proprietà {proprieta.Name} è obbligatoria ma non ha valore.");
            }
            else if (valore is string testo && string.IsNullOrWhiteSpace(testo))
            {
                errori.Add($"La proprietà {proprieta.Name} è obbligatoria e non può essere vuota.");
            }

            // TODO: se necessario aggiungi altri controlli specifici per il tuo dominio.
        }

        return errori.Count == 0;
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Step 2: Reflection e IValidabile ==\n");

        var utenteValido = new Utente
        {
            Nome = "Ada",
            Email = "ada.lovelace@example.com",
            Note = "Il primo algoritmo destinato a una macchina."
        };

        var utenteNonValido = new Utente
        {
            Nome = "Grace",
            Email = "", // TODO: prova a lasciare null o vuoto per generare un errore.
            Note = "Pioniera del linguaggio COBOL."
        };

        Verifica(utenteValido);
        Console.WriteLine();
        Verifica(utenteNonValido);

        // Esempio di output atteso:
        // Utente Ada => valido: True
        // Utente Grace => valido: False
        //  - La proprietà Email è obbligatoria e non può essere vuota.
    }

    private static void Verifica(Utente utente)
    {
        var risultato = utente.Valida(out var errori);
        Console.WriteLine($"Utente {utente.Nome ?? "(senza nome)"} => valido: {risultato}");

        foreach (var errore in errori)
        {
            Console.WriteLine($" - {errore}");
        }
    }
}
