// Step 3: Validatore generico – Separiamo la logica di validazione dal modello.
// In questo step creiamo una classe Validatore<T> con vincolo where T : IValidabile,
// così da poter riutilizzare la stessa infrastruttura con qualunque modello validabile.

using System.Reflection;

namespace ValidazioneModelliGuidato.Step3;

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
            if (!Attribute.IsDefined(proprieta, typeof(ObbligatorioAttribute)))
            {
                continue;
            }

            var valore = proprieta.GetValue(this);

            if (valore is null)
            {
                errori.Add($"{proprieta.Name} è obbligatoria.");
            }
            else if (valore is string testo && string.IsNullOrWhiteSpace(testo))
            {
                errori.Add($"{proprieta.Name} non può essere vuota.");
            }
        }

        return errori.Count == 0;
    }
}

public class Validatore<T> where T : IValidabile
{
    public bool Valida(T istanza, out List<string> errori)
    {
        if (istanza is null)
        {
            throw new ArgumentNullException(nameof(istanza));
        }

        // TODO: delega la responsabilità della validazione all'istanza stessa.
        // Suggerimento: richiama il metodo Valida definito dall'interfaccia.
        return istanza.Valida(out errori);
    }

    public IEnumerable<EsitoValidazione<T>> ValidaTutti(IEnumerable<T> istanze)
    {
        if (istanze is null)
        {
            throw new ArgumentNullException(nameof(istanze));
        }

        foreach (var elemento in istanze)
        {
            // TODO: riutilizza il metodo Valida singolo per evitare duplicazione.
            var valido = Valida(elemento, out var errori);
            yield return new EsitoValidazione<T>(elemento, valido, errori);
        }
    }
}

public record EsitoValidazione<T>(T Istanza, bool Valido, List<string> Errori);

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("== Step 3: Validatore generico ==\n");

        var utenti = new List<Utente>
        {
            new()
            {
                Nome = "Ada",
                Email = "ada.lovelace@example.com"
            },
            new()
            {
                Nome = "Alan",
                Email = ""
            },
            new()
            {
                Nome = null,
                Email = "grace.hopper@example.com"
            }
        };

        var validatore = new Validatore<Utente>();
        var risultati = validatore.ValidaTutti(utenti);

        foreach (var esito in risultati)
        {
            Console.WriteLine($"Utente: {esito.Istanza.Nome ?? "(senza nome)"} -> Valido: {esito.Valido}");
            foreach (var errore in esito.Errori)
            {
                Console.WriteLine($"   - {errore}");
            }
        }

        // Esempio di output sintetico:
        // Utente: Ada -> Valido: True
        // Utente: Alan -> Valido: False
        //    - Email non può essere vuota.
        // Utente: (senza nome) -> Valido: False
        //    - Nome è obbligatoria.
    }
}
