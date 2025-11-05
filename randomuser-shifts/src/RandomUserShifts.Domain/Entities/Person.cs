namespace RandomUserShifts.Domain.Entities;

/// <summary>
/// Represents a worker that can be assigned to a shift.
/// </summary>
public sealed record Person(
    Guid Id,
    string FullName,
    string Email,
    string Nationality,
    int SkillLevel = 1);
