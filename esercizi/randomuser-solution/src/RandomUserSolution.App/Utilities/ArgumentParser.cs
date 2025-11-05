// ---------------------------------------------------------------------------------------------------------------------
// ArgumentParser.cs
// ---------------------------------------------------------------------------------------------------------------------
// Parsing manuale degli argomenti della CLI. L'obiettivo è offrire un esempio leggibile senza ricorrere a
// librerie esterne: riconosciamo coppie del tipo "--chiave valore" e produciamo un oggetto AppOptions.
// ---------------------------------------------------------------------------------------------------------------------

using RandomUserSolution.App.Models;

namespace RandomUserSolution.App.Utilities;

/// <summary>
/// Converte l'array di stringhe fornito a Main in un'istanza di <see cref="AppOptions"/>.
/// </summary>
public sealed class ArgumentParser
{
    private static readonly HashSet<string> SupportedArguments = new(StringComparer.OrdinalIgnoreCase)
    {
        "--count",
        "--nat",
        "--gender",
        "--seed"
    };

    /// <summary>
    /// Esegue il parsing degli argomenti. In caso di input non valido solleva <see cref="ArgumentException"/>.
    /// </summary>
    public AppOptions Parse(string[] args)
    {
        // Valori di default: 5 persone, nessun filtro e seed nullo.
        var requestedPeople = 5;
        string? nationality = null;
        string? gender = null;
        string? seed = null;

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];

            if (!SupportedArguments.Contains(argument))
            {
                throw new ArgumentException($"Argomento non riconosciuto: {argument}");
            }

            if (index + 1 >= args.Length)
            {
                throw new ArgumentException($"L'argomento {argument} richiede un valore.");
            }

            var value = args[++index];

            switch (argument.ToLowerInvariant())
            {
                case "--count":
                    if (!int.TryParse(value, out requestedPeople) || requestedPeople <= 0)
                    {
                        throw new ArgumentException("--count deve essere un intero positivo.");
                    }
                    break;
                case "--nat":
                    nationality = value.ToUpperInvariant();
                    break;
                case "--gender":
                    var normalizedGender = value.ToLowerInvariant();
                    if (normalizedGender is not ("male" or "female"))
                    {
                        throw new ArgumentException("--gender supporta solo i valori 'male' e 'female'.");
                    }
                    gender = normalizedGender;
                    break;
                case "--seed":
                    seed = value;
                    break;
            }
        }

        return new AppOptions(requestedPeople, nationality, gender, seed);
    }
}
