using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;
using RandomUserShifts.Strategies;
using Xunit;

namespace RandomUserShifts.Tests;

public sealed class StrategyFairnessTests
{
    [Fact]
    public void Build_MinimizesDifferenceBetweenPeople()
    {
        // Arrange: la strategia fairness dovrebbe bilanciare il numero di turni.
        var strategy = new FairnessStrategy();
        var weekStart = new DateOnly(2025, 1, 13);
        var rules = new SchedulingRules();
        var people = new List<Person>
        {
            new(Guid.NewGuid(), "Alice Example", "alice@example.com", "IT"),
            new(Guid.NewGuid(), "Bob Example", "bob@example.com", "IT"),
            new(Guid.NewGuid(), "Carol Example", "carol@example.com", "IT")
        };

        // Act
        var schedule = strategy.Build(weekStart, people, rules);

        // Assert: rimane rosso finché TODO[3] non distribuisce equamente i turni.
        Assert.Equal(14, schedule.Shifts.Count);
        var shiftsPerPerson = schedule.Shifts
            .GroupBy(s => s.AssignedPersonId)
            .ToDictionary(g => g.Key, g => g.Count());

        Assert.Equal(people.Count, shiftsPerPerson.Count);
        var max = shiftsPerPerson.Values.Max();
        var min = shiftsPerPerson.Values.Min();
        Assert.True(max - min <= 1, "Differenza tra persone troppo alta: la fairness non è rispettata");
    }
}
