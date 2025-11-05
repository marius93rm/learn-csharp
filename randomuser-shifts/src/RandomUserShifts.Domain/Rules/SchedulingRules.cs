namespace RandomUserShifts.Domain.Rules;

/// <summary>
/// Defines constraints applied while generating schedules.
/// </summary>
public sealed class SchedulingRules
{
    public int MaxShiftsPerDayPerPerson { get; init; } = 1;

    public TimeSpan MinimumRestBetweenShifts { get; init; } = TimeSpan.FromHours(12);

    public int RequiredShiftsPerDay { get; init; } = 2;

    public IReadOnlyCollection<string> Describe()
    {
        return new[]
        {
            $"Max {MaxShiftsPerDayPerPerson} shift(s) per day per person",
            $"Minimum rest {MinimumRestBetweenShifts.TotalHours}h between shifts",
            $"Required shifts per day: {RequiredShiftsPerDay}"
        };
    }
}
