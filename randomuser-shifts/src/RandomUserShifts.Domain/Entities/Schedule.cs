namespace RandomUserShifts.Domain.Entities;

/// <summary>
/// Weekly schedule containing all shifts and metadata.
/// </summary>
public sealed class Schedule
{
    public Schedule(DateOnly weekStartDate, IReadOnlyCollection<Shift> shifts, IReadOnlyCollection<string> rulesApplied)
    {
        WeekStartDate = weekStartDate;
        Shifts = shifts;
        RulesApplied = rulesApplied;
    }

    public DateOnly WeekStartDate { get; }

    public IReadOnlyCollection<Shift> Shifts { get; }

    public IReadOnlyCollection<string> RulesApplied { get; }
}
