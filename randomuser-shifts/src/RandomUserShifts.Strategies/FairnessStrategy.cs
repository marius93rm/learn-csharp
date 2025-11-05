using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;

namespace RandomUserShifts.Strategies;

public sealed class FairnessStrategy : ISchedulingStrategy
{
    public Schedule Build(DateOnly weekStart, IReadOnlyList<Person> people, SchedulingRules rules)
    {
        // TODO[3]: assegna minimizzando la varianza ore/persona mantenendo le regole (abbozza algoritmo).
        return new Schedule(weekStart, Array.Empty<Shift>(), rules.Describe());
    }
}
