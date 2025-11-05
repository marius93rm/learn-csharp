using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;
using RandomUserShifts.Strategies;
using Xunit;

namespace RandomUserShifts.Tests;

public sealed class StrategyRoundRobinTests
{
    private static IReadOnlyList<Person> CreatePeople(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new Person(Guid.NewGuid(), $"Person {i}", $"person{i}@example.com", "IT"))
            .ToList();
    }

    [Fact]
    public void Build_GeneratesFourteenShiftsRespectingRules()
    {
        // Arrange: 7 giorni * 2 turni = 14 assegnazioni da distribuire round-robin.
        var strategy = new RoundRobinStrategy();
        var weekStart = new DateOnly(2025, 1, 13);
        var people = CreatePeople(4);
        var rules = new SchedulingRules();

        // Act
        var schedule = strategy.Build(weekStart, people, rules);

        // Assert: fallisce finché TODO[2] non ruota correttamente le persone.
        Assert.Equal(14, schedule.Shifts.Count);
        foreach (var dayGroup in schedule.Shifts.GroupBy(s => s.DayOfWeek))
        {
            Assert.Equal(rules.RequiredShiftsPerDay, dayGroup.Count());
            foreach (var group in dayGroup.GroupBy(s => s.AssignedPersonId))
            {
                Assert.True(group.Count() <= rules.MaxShiftsPerDayPerPerson, "Una persona ha troppi turni nello stesso giorno");
            }
        }

        var orderedAssignments = schedule.Shifts
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.Start)
            .Select(s => s.AssignedPersonId)
            .ToList();

        var uniqueSequences = orderedAssignments.Distinct().Count();
        Assert.True(uniqueSequences <= people.Count, "Il round-robin deve riutilizzare tutti i lavoratori prima di ricominciare");
    }
}
