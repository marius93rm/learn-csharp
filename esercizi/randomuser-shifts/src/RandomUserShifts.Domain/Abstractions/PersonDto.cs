namespace RandomUserShifts.Domain.Abstractions;

/// <summary>
/// Lightweight representation of a person returned by the RandomUser API adapter.
/// </summary>
public sealed record PersonDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Nationality);
