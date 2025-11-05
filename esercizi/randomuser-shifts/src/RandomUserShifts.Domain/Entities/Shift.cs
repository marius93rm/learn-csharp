namespace RandomUserShifts.Domain.Entities;

/// <summary>
/// Represents a single shift assignment within a week.
/// </summary>
public sealed class Shift
{
    public Shift(DayOfWeek dayOfWeek, TimeOnly start, TimeOnly end, Guid? assignedPersonId)
    {
        DayOfWeek = dayOfWeek;
        Start = start;
        End = end;
        AssignedPersonId = assignedPersonId;
    }

    public DayOfWeek DayOfWeek { get; }

    public TimeOnly Start { get; }

    public TimeOnly End { get; }

    public Guid? AssignedPersonId { get; }

    public TimeSpan Duration
    {
        get
        {
            var startDateTime = DateTime.Today.Add(Start.ToTimeSpan());
            var endDateTime = DateTime.Today.Add(End.ToTimeSpan());
            if (endDateTime <= startDateTime)
            {
                endDateTime = endDateTime.AddDays(1);
            }

            return endDateTime - startDateTime;
        }
    }
}
