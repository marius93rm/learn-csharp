// Step 1: Attributo Obbligatorio – In questo step definisci un attributo personalizzato
// per dichiarare quali proprietà del modello devono essere presenti. Il codice è già
// eseguibile, ma include TODO da completare per consolidare l'apprendimento.

namespace ValidazioneModelliGuidato.Step1;

// L'attributo verrà letto tramite reflection nei prossimi step.
[AttributeUsage(AttributeTargets.Property)]
public sealed class ObbligatorioAttribute : Attribute
{
    // TODO: (Opzionale) aggiungi qui proprietà o costruttori per personalizzare
    // il messaggio d'errore quando la proprietà non è valorizzata.
}

public class Utente
{
    // TODO: aggiungi [Obbligatorio] alle proprietà che ritieni indispensabili.
    // Ricorda: gli attributi si scrivono sopra la proprietà.
    public string? Nome { get; set; }

    public string? Email { get; set; }

    // Proprietà senza obbligo, utile per confrontare il comportamento.
    public string? Note { get; set; }

    public override string ToString() => $"{Nome} - {Email ?? "(email mancante)"}";
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Step 1: Attributo Obbligatorio ==\n");
        Console.WriteLine("1) Aggiungi [Obbligatorio] alle proprietà richieste del modello Utente.");
        Console.WriteLine("2) Esegui il programma per osservare come leggere gli attributi con reflection.\n");

        var utente = new Utente
        {
            // TODO: prova a commentare Nome o Email per vedere, nello step successivo,
            // come cambierà l'esito della validazione automatica.
            Nome = "Ada",
            Email = "ada.lovelace@example.com",
            Note = "Prima programmatrice documentata."
        };

        Console.WriteLine($"Utente creato: {utente}");
        Console.WriteLine("Attributi applicati alle proprietà di Utente:\n");

        foreach (var proprieta in typeof(Utente).GetProperties())
        {
            var haObbligatorio = Attribute.IsDefined(proprieta, typeof(ObbligatorioAttribute));
            Console.WriteLine($" - {proprieta.Name}: {(haObbligatorio ? "[Obbligatorio]" : "facoltativo")}");
        }

        // Output atteso (dopo aver aggiunto gli attributi obbligatori):
        // - Nome: [Obbligatorio]
        // - Email: [Obbligatorio]
        // - Note: facoltativo
    }
}
