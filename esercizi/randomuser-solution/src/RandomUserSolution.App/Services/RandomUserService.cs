// ---------------------------------------------------------------------------------------------------------------------
// RandomUserService.cs
// ---------------------------------------------------------------------------------------------------------------------
// Questa classe incapsula la logica per comunicare con l'API randomuser.me. Separare la logica di accesso
// ai dati (HTTP + JSON) dal resto dell'applicazione ci permette di testare e riutilizzare il codice più
// facilmente e rende il programma principale (Program.cs) più leggibile.
// ---------------------------------------------------------------------------------------------------------------------

using System.Text;
using System.Text.Json;
using RandomUserSolution.App.Models;

namespace RandomUserSolution.App.Services;

/// <summary>
/// Servizio responsabile del download e della proiezione degli utenti casuali.
/// </summary>
public sealed class RandomUserService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true // Consente di deserializzare JSON con casing diverso da quello C#.
    };

    private readonly HttpClient _httpClient;

    public RandomUserService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Factory method che configura un <see cref="HttpClient"/> con le impostazioni consigliate per l'API RandomUser.
    /// </summary>
    public static HttpClient CreateHttpClient()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri("https://randomuser.me/", UriKind.Absolute)
        };

        // È buona pratica includere uno user-agent descrittivo.
        client.DefaultRequestHeaders.UserAgent.ParseAdd("RandomUserSolution/1.0 (+https://randomuser.me)");

        return client;
    }

    /// <summary>
    /// Scarica una collezione di utenti fittizi rispettando i filtri opzionali specificati.
    /// </summary>
    /// <param name="count">Numero di utenti richiesti (1-500 secondo la documentazione ufficiale).</param>
    /// <param name="nationality">Filtro sulla nazionalità (es. "IT" oppure "US").</param>
    /// <param name="gender">Filtro sul genere, "male" o "female".</param>
    /// <param name="seed">Seed opzionale per rendere deterministica la risposta.</param>
    /// <exception cref="ArgumentOutOfRangeException">Se il numero richiesto è minore o uguale a zero.</exception>
    /// <exception cref="RandomUserServiceException">Per problemi di deserializzazione o risposta inattesa.</exception>
    public async Task<IReadOnlyList<RandomUser>> GetRandomUsersAsync(int count, string? nationality, string? gender, string? seed)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Devi richiedere almeno una persona.");
        }

        // Costruiamo la query string manualmente per avere il pieno controllo sui parametri opzionali.
        var queryBuilder = new StringBuilder("api/?");
        queryBuilder.Append("results=").Append(count);

        if (!string.IsNullOrWhiteSpace(nationality))
        {
            queryBuilder.Append("&nat=").Append(Uri.EscapeDataString(nationality));
        }

        if (!string.IsNullOrWhiteSpace(gender))
        {
            queryBuilder.Append("&gender=").Append(Uri.EscapeDataString(gender));
        }

        if (!string.IsNullOrWhiteSpace(seed))
        {
            queryBuilder.Append("&seed=").Append(Uri.EscapeDataString(seed));
        }

        using var response = await _httpClient.GetAsync(queryBuilder.ToString());
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        var payload = await JsonSerializer.DeserializeAsync<RandomUserApiResponse>(stream, SerializerOptions);

        if (payload?.Results is null)
        {
            throw new RandomUserServiceException("La risposta dell'API non contiene il campo 'results'.");
        }

        // Proiettiamo i dati ricevuti in oggetti di dominio leggibili dall'applicazione.
        var people = payload.Results.Select(MapToDomain).ToList();

        if (people.Count == 0)
        {
            throw new RandomUserServiceException("L'API ha restituito zero risultati.");
        }

        return people;
    }

    private static RandomUser MapToDomain(RandomUserApiResult result)
    {
        // Componiamo il nome completo rispettando titolo, nome e cognome.
        var fullName = $"{result.Name.Title} {result.Name.First} {result.Name.Last}".Trim();

        return new RandomUser
        {
            FullName = fullName,
            Email = result.Email,
            Nationality = result.Nationality,
            Gender = result.Gender,
            Phone = result.Phone
        };
    }
}

/// <summary>
/// Eccezione personalizzata usata per distinguere gli errori funzionali da quelli di rete.
/// </summary>
public sealed class RandomUserServiceException : Exception
{
    public RandomUserServiceException(string message)
        : base(message)
    {
    }
}
