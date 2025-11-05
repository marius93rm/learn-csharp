// ---------------------------------------------------------------------------------------------------------------------
// RandomUser.cs
// ---------------------------------------------------------------------------------------------------------------------
// Questa classe rappresenta la nostra "vista" dell'utente restituito da randomuser.me. È intenzionalmente
// più semplice rispetto al JSON originale e contiene solo i campi utili per l'esercizio.
// ---------------------------------------------------------------------------------------------------------------------

namespace RandomUserSolution.App.Models;

/// <summary>
/// Rappresenta un utente fittizio ottenuto dal servizio RandomUser.
/// </summary>
public sealed class RandomUser
{
    /// <summary>
    /// Nome completo già formattato (es. "Ada Lovelace").
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Indirizzo email generato dal servizio.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Paese / nazionalità dell'utente (codice ISO a due lettere).
    /// </summary>
    public required string Nationality { get; init; }

    /// <summary>
    /// Genere dell'utente, esattamente come restituito dall'API ("male" o "female").
    /// </summary>
    public required string Gender { get; init; }

    /// <summary>
    /// Numero di telefono formattato. Non è fondamentale ma rende la tabella più interessante.
    /// </summary>
    public required string Phone { get; init; }
}
