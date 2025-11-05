// ---------------------------------------------------------------------------------------------------------------------
// RandomUserApiResponse.cs
// ---------------------------------------------------------------------------------------------------------------------
// Serie di record che riflettono (parzialmente) la struttura JSON restituita dall'endpoint
// https://randomuser.me/api/. Li usiamo esclusivamente per la deserializzazione tramite System.Text.Json.
// ---------------------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace RandomUserSolution.App.Models;

/// <summary>
/// Record principale che rappresenta la risposta JSON dell'API.
/// </summary>
public sealed record RandomUserApiResponse([
    property: JsonPropertyName("results")
] IReadOnlyList<RandomUserApiResult> Results);

/// <summary>
/// Ciascun elemento della collezione "results" contiene un utente fittizio con molte informazioni.
/// </summary>
public sealed record RandomUserApiResult(
    [property: JsonPropertyName("name")] RandomUserApiName Name,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("nat")] string Nationality,
    [property: JsonPropertyName("gender")] string Gender,
    [property: JsonPropertyName("phone")] string Phone
);

/// <summary>
/// Sottostruttura che contiene nome proprio, cognome, ecc.
/// </summary>
public sealed record RandomUserApiName(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("first")] string First,
    [property: JsonPropertyName("last")] string Last
);
