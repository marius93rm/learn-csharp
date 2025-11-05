using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;
using RandomUserShifts.Domain.Services;
using Xunit;

namespace RandomUserShifts.Tests;

public sealed class SchedulerTests
{
    [Fact]
    public void Build_WithNoPeople_ThrowsInvalidOperation()
    {
        // Arrange
        var scheduler = new Scheduler();
        var rules = new SchedulingRules();
        var strategy = new StubStrategy();

        // Act & Assert: rosso finché TODO[4] non valida l'input.
        Assert.Throws<InvalidOperationException>(() => scheduler.Build(new DateOnly(2025, 1, 13), Array.Empty<Person>(), rules, strategy));
        Assert.False(strategy.Invoked, "La strategia non dovrebbe essere chiamata quando mancano le persone");
    }

    [Fact]
    public void Build_AppendsRulesDescription()
    {
        // Arrange: la strategia restituisce un schedule senza regole.
        var scheduler = new Scheduler();
        var rules = new SchedulingRules { MaxShiftsPerDayPerPerson = 1 };
        var strategy = new StubStrategy();
        var people = new[] { new Person(Guid.NewGuid(), "Test", "test@example.com", "IT") };

        // Act
        var schedule = scheduler.Build(new DateOnly(2025, 1, 13), people, rules, strategy);

        // Assert: rosso finché TODO[4] non aggiunge le regole applicate.
        Assert.NotEmpty(schedule.RulesApplied);
        Assert.Contains("Max", schedule.RulesApplied.First());
    }

    private sealed class StubStrategy : ISchedulingStrategy
    {
        public bool Invoked { get; private set; }

        public Schedule Build(DateOnly weekStart, IReadOnlyList<Person> people, SchedulingRules rules)
        {
            Invoked = true;
            return new Schedule(weekStart, Array.Empty<Shift>(), Array.Empty<string>());
        }
    }
}
