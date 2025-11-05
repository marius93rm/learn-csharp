using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;

namespace RandomUserShifts.Domain.Abstractions;

public interface ISchedulingStrategy
{
    Schedule Build(DateOnly weekStart, IReadOnlyList<Person> people, SchedulingRules rules);
}
