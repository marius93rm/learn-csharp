using FluentAssertions;
using Pomodoro.App.Core;
using Pomodoro.App.Notify;
using Pomodoro.App.Persistence;
using Pomodoro.App.Sessions;
using Pomodoro.App.Timer;
using Xunit;
using Xunit.Abstractions;

namespace Pomodoro.Tests;

public class PomodoroTests
{
    private readonly ITestOutputHelper _output;

    public PomodoroTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task TimerService_CountdownAsync_Should_Invoke_Callbacks()
    {
        var tickProvider = new ImmediateTickProvider();
        var timerService = new TimerService(tickProvider);
        var ticks = new List<int>();
        var completed = false;

        await timerService.CountdownAsync(
            totalSeconds: 3,
            onTick: ticks.Add,
            onCompleted: () => completed = true,
            cancellationToken: CancellationToken.None);

        ticks.Should().Equal(3, 2, 1);
        completed.Should().BeTrue();

        _output.WriteLine("TimerService_CountdownAsync passed");
    }

    [Fact]
    public async Task Pomodoro_RunAsync_Should_Raise_Event_Notify_And_Log()
    {
        var tickProvider = new ImmediateTickProvider();
        var timerService = new TimerService(tickProvider);
        var strategy = new Custom("Test", focusSeconds: 2, breakSeconds: 1);
        var notifier = new RecordingNotifier();
        var repository = new RecordingRepository();
        var pomodoro = new Pomodoro(timerService, strategy, new[] { notifier }, repository);
        var ticks = new List<int>();
        var focusCompleted = false;
        pomodoro.FocusCompleted += (_, _) => focusCompleted = true;

        await pomodoro.RunAsync(ticks.Add, CancellationToken.None);

        ticks.Should().ContainInOrder(2, 1);
        focusCompleted.Should().BeTrue();
        notifier.Messages.Should().NotBeEmpty();
        repository.Sessions.Should().ContainSingle(session =>
            session.FocusSeconds == 2 &&
            session.BreakSeconds == 1 &&
            session.StrategyName == "Test");

        _output.WriteLine("Pomodoro_RunAsync passed");
    }

    [Fact]
    public void Strategies_Should_Return_Expected_Durations()
    {
        var classic = new Classic25_5();
        var deep = new Deep50_10();
        var custom = new Custom("Custom", 10, 20);

        classic.GetDurations().Should().Be((25 * 60, 5 * 60));
        deep.GetDurations().Should().Be((50 * 60, 10 * 60));
        custom.GetDurations().Should().Be((10, 20));

        _output.WriteLine("Strategies durations passed");
    }

    [Fact]
    public async Task CsvSessionRepository_Should_Append_Csv_Line()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"pomodoro_{Guid.NewGuid():N}.csv");
        try
        {
            var repository = new CsvSessionRepository(tempFile);
            var session = new PomodoroSessionLog(
                new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                FocusSeconds: 1500,
                BreakSeconds: 300,
                StrategyName: "Classic 25/5");

            await repository.SaveAsync(session, CancellationToken.None);

            File.Exists(tempFile).Should().BeTrue();
            var lines = await File.ReadAllLinesAsync(tempFile);
            lines.Should().HaveCount(1);
            lines[0].Should().Contain("1500;300;Classic 25/5");

            _output.WriteLine("CsvSessionRepository append passed");
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    private sealed class ImmediateTickProvider : ITickProvider
    {
        public Task DelayAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingNotifier : INotifier
    {
        private readonly List<string> _messages = new();

        public IReadOnlyList<string> Messages => _messages;

        public void Notify(string message)
        {
            _messages.Add(message);
        }
    }

    private sealed class RecordingRepository : ISessionRepository
    {
        private readonly List<PomodoroSessionLog> _sessions = new();

        public IReadOnlyList<PomodoroSessionLog> Sessions => _sessions;

        public Task SaveAsync(PomodoroSessionLog session, CancellationToken cancellationToken = default)
        {
            _sessions.Add(session);
            return Task.CompletedTask;
        }
    }
}
