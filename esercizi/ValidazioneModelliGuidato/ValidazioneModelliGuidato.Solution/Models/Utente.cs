using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ValidazioneModelliGuidato.Solution.Validation;
using ValidazioneModelliGuidato.Solution.Validation.Attributes;

namespace ValidazioneModelliGuidato.Solution.Models;

/// <summary>
/// Modello di dominio utilizzato come esempio per il sistema di validazione.
/// </summary>
public class Utente : IValidabile
{
    [Obbligatorio]
    public string? Nome { get; set; }

    [Obbligatorio("L'indirizzo email è un campo obbligatorio per contattare l'utente.")]
    public string? Email { get; set; }

    [Obbligatorio]
    [MinLunghezza(8)]
    public string? Password { get; set; }

    [MinLunghezza(1)]
    public List<string> Ruoli { get; init; } = new();

    public override string ToString() => Nome ?? "(utente senza nome)";

    /// <summary>
    /// Implementazione dell'interfaccia <see cref="IValidabile"/> basata sulla reflection.
    /// </summary>
    public bool Valida(out List<MessaggioValidazione> messaggi)
    {
        messaggi = new List<MessaggioValidazione>();

        // Recupera tutte le proprietà pubbliche d'istanza (SRP: il modello conosce i propri attributi).
        foreach (var proprieta in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var valore = proprieta.GetValue(this);

            // Selezioniamo solo gli attributi che implementano IRegolaValidazione.
            var regole = proprieta.GetCustomAttributes().OfType<IRegolaValidazione>();

            foreach (var regola in regole)
            {
                var esito = regola.Verifica(valore, proprieta);
                if (esito is not null)
                {
                    messaggi.Add(esito.Value);
                }
            }
        }

        // Il modello è valido solo se non esistono errori.
        return messaggi.All(m => m.Livello != LivelloMessaggio.Errore);
    }
}
