// Step 4: Nuovo attributo e OCP – Aggiungiamo [MinLunghezza] senza toccare il validatore.
// L'obiettivo è estendere il sistema introducendo una regola aggiuntiva mantenendo SRP
// (modello e validatore separati) e OCP (aggiungiamo attributi senza modificare il Validatore<T>).

using System.Reflection;

namespace ValidazioneModelliGuidato.Step4;

public interface IValidabile
{
    bool Valida(out List<string> errori);
}

public interface IRegolaValidazione
{
    bool Verifica(object? valore, PropertyInfo proprieta, out string? messaggio);
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class ObbligatorioAttribute : Attribute, IRegolaValidazione
{
    public bool Verifica(object? valore, PropertyInfo proprieta, out string? messaggio)
    {
        if (valore is null)
        {
            messaggio = $"La proprietà {proprieta.Name} è obbligatoria.";
            return false;
        }

        if (valore is string testo && string.IsNullOrWhiteSpace(testo))
        {
            messaggio = $"La proprietà {proprieta.Name} non può essere vuota.";
            return false;
        }

        messaggio = null;
        return true;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MinLunghezzaAttribute : Attribute, IRegolaValidazione
{
    public MinLunghezzaAttribute(int lunghezzaMinima)
    {
        LunghezzaMinima = lunghezzaMinima;
    }

    public int LunghezzaMinima { get; }

    public bool Verifica(object? valore, PropertyInfo proprieta, out string? messaggio)
    {
        // TODO: estendi la regola se vuoi supportare anche altri tipi (es. ICollection).
        if (valore is string testo)
        {
            if (testo.Length < LunghezzaMinima)
            {
                messaggio = $"La proprietà {proprieta.Name} deve avere almeno {LunghezzaMinima} caratteri.";
                return false;
            }

            messaggio = null;
            return true;
        }

        // Se l'attributo viene applicato a un tipo non stringa non generiamo errore, ma segnaliamo.
        messaggio = $"[Avviso] {nameof(MinLunghezzaAttribute)} è pensato per le stringhe.";
        return false;
    }
}

public class Utente : IValidabile
{
    [Obbligatorio]
    public string? Nome { get; set; }

    [Obbligatorio]
    public string? Email { get; set; }

    [Obbligatorio]
    [MinLunghezza(8)]
    public string? Password { get; set; }

    public bool Valida(out List<string> errori)
    {
        errori = new List<string>();

        foreach (var proprieta in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var valore = proprieta.GetValue(this);
            var attributi = proprieta.GetCustomAttributes().OfType<IRegolaValidazione>();

            foreach (var regola in attributi)
            {
                // TODO: separa eventuali messaggi di warning dagli errori se vuoi tracciare livelli diversi.
                if (!regola.Verifica(valore, proprieta, out var messaggio) && messaggio is not null)
                {
                    errori.Add(messaggio);
                }
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
        Console.WriteLine("== Step 4: Nuovo attributo MinLunghezza ==\n");
        Console.WriteLine("Obiettivo: aggiungi nuove regole tramite attributi senza cambiare Validatore<T>.\n");

        var utenti = new List<Utente>
        {
            new()
            {
                Nome = "Ada",
                Email = "ada.lovelace@example.com",
                Password = "Analytical" // 11 caratteri, valido.
            },
            new()
            {
                Nome = "Alan",
                Email = "alan.turing@example.com",
                Password = "Enigma" // TODO: modifica per testare la lunghezza minima.
            },
            new()
            {
                Nome = "Grace",
                Email = null, // TODO: rendi null per osservare la combinazione di errori.
                Password = "Cobol2023"
            }
        };

        var validatore = new Validatore<Utente>();

        foreach (var esito in validatore.ValidaTutti(utenti))
        {
            Console.WriteLine($"Utente: {esito.Istanza.Nome ?? "(senza nome)"} -> Valido: {esito.Valido}");
            foreach (var errore in esito.Errori)
            {
                Console.WriteLine($"   - {errore}");
            }

            Console.WriteLine();
        }

        // Esempio di output sintetico:
        // Utente: Ada -> Valido: True
        // Utente: Alan -> Valido: False
        //    - La proprietà Password deve avere almeno 8 caratteri.
        // Utente: Grace -> Valido: False
        //    - La proprietà Email è obbligatoria.
    }
}
