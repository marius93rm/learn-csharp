using RandomUserShifts.App.Cli;
using RandomUserShifts.App.Composition;
using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;
using RandomUserShifts.Domain.Services;
using Xunit;

namespace RandomUserShifts.Tests;

public sealed class ConsoleOutputTests
{
    [Fact]
    public async Task Program_PrintsWeeklySummary()
    {
        // Arrange: componiamo il contesto manualmente per controllare l'output.
        var writer = new FakeConsoleWriter();
        var strategy = new FakeStrategy();
        var context = new BootstrapContext(
            new FakeRandomUserClient(),
            new FakeScheduleRepository(),
            new Dictionary<string, ISchedulingStrategy>
            {
                ["round-robin"] = strategy
            },
            new Scheduler(),
            writer);

        var args = new[] { "--people", "2", "--week", "2025-01-13", "--strategy", "round-robin" };

        // Act
        await ProgramEntrypoint.RunAsync(args, context);

        // Assert: fallisce finché TODO[7] non stampa i turni giornalieri e i totali.
        Assert.Contains(writer.Lines, line => line.StartsWith("RandomUser Shifts"));
        Assert.True(writer.Lines.Any(line => line.StartsWith("Mon")), "Dovrebbe esserci una riga per il lunedì");
        Assert.True(writer.Lines.Count(line => line.Contains(":")) >= 7, "Serve una riga per ciascun giorno della settimana");
        Assert.True(writer.Lines.Any(line => line.Contains("Totali")), "Serve un riepilogo con i totali per persona");
    }

    private sealed class FakeRandomUserClient : IRandomUserClient
    {
        private readonly IReadOnlyList<PersonDto> _people = new List<PersonDto>
        {
            new(Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeffffffff"), "Ada", "Lovelace", "ada@example.com", "GB"),
            new(Guid.Parse("bbbbbbbb-cccc-dddd-eeee-ffffffff0000"), "Grace", "Hopper", "grace@example.com", "US")
        };

        public Task<IReadOnlyList<PersonDto>> GetRandomUsersAsync(int count, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_people);
        }
    }

    private sealed class FakeScheduleRepository : IScheduleRepository
    {
        public Schedule? SavedSchedule { get; private set; }

        public Task SaveAsync(Schedule schedule, CancellationToken cancellationToken = default)
        {
            SavedSchedule = schedule;
            return Task.CompletedTask;
        }

        public Task<Schedule?> LoadAsync(DateOnly weekStart, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Schedule?>(SavedSchedule);
        }
    }

    private sealed class FakeStrategy : ISchedulingStrategy
    {
        public Schedule Build(DateOnly weekStart, IReadOnlyList<Person> people, SchedulingRules rules)
        {
            var shifts = new List<Shift>();
            var morning = new TimeOnly(8, 0);
            var evening = new TimeOnly(16, 0);

            foreach (var day in Enum.GetValues<DayOfWeek>())
            {
                shifts.Add(new Shift(day, morning, evening, people[0].Id));
                shifts.Add(new Shift(day, evening, new TimeOnly(0, 0), people[1].Id));
            }

            return new Schedule(weekStart, shifts, rules.Describe());
        }
    }

    private sealed class FakeConsoleWriter : IConsoleWriter
    {
        public List<string> Lines { get; } = new();

        public void WriteLine(string value)
        {
            Lines.Add(value);
        }
    }
}
