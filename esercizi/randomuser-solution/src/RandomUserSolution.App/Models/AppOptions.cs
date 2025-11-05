// ---------------------------------------------------------------------------------------------------------------------
// AppOptions.cs
// ---------------------------------------------------------------------------------------------------------------------
// Il record AppOptions rappresenta tutte le opzioni che l'utente può configurare da linea di comando.
// Mantenerle in una singola struttura semplifica il passaggio delle informazioni tra i vari componenti
// dell'applicazione (Program, ConsoleTableFormatter, RandomUserService, ...).
// ---------------------------------------------------------------------------------------------------------------------

namespace RandomUserSolution.App.Models;

/// <summary>
/// Raccoglie le impostazioni della CLI.
/// </summary>
/// <param name="RequestedPeople">Numero di persone da richiedere all'API RandomUser.</param>
/// <param name="NationalityFilter">Codice di nazionalità (es. "US" o "IT") da passare come filtro. Può essere <c>null</c>.</param>
/// <param name="GenderFilter">Filtro sul genere ("male" oppure "female"). Può essere <c>null</c>.</param>
/// <param name="Seed">Parametro opzionale che stabilizza la risposta dell'API (utile per test ripetibili).</param>
public sealed record AppOptions(
    int RequestedPeople,
    string? NationalityFilter,
    string? GenderFilter,
    string? Seed);
