using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;

namespace RandomUserShifts.Strategies;

public sealed class RoundRobinStrategy : ISchedulingStrategy
{
    public Schedule Build(DateOnly weekStart, IReadOnlyList<Person> people, SchedulingRules rules)
    {
        // TODO[2]: distribuisci i turni 7x2 (mattina/sera) ruotando le persone e rispettando SchedulingRules.
        return new Schedule(weekStart, Array.Empty<Shift>(), rules.Describe());
    }
}
