using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;

namespace RandomUserShifts.Domain.Services;

/// <summary>
/// Coordinates strategy execution while keeping the domain logic free of infrastructure concerns.
/// </summary>
public sealed class Scheduler
{
    public Schedule Build(DateOnly weekStart, IReadOnlyList<Person> people, SchedulingRules rules, ISchedulingStrategy strategy)
    {
        // TODO[4]: valida input (almeno 1 persona), applica strategy e annota rulesApplied.
        return strategy.Build(weekStart, people, rules);
    }
}
