using RandomUserShifts.App.Cli;
using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Services;
using RandomUserShifts.Infrastructure.Http;
using RandomUserShifts.Infrastructure.Persistence;
using RandomUserShifts.Strategies;

namespace RandomUserShifts.App.Composition;

public sealed record BootstrapContext(
    IRandomUserClient RandomUserClient,
    IScheduleRepository ScheduleRepository,
    IReadOnlyDictionary<string, ISchedulingStrategy> Strategies,
    Scheduler Scheduler,
    IConsoleWriter ConsoleWriter);

public static class Bootstrap
{
    public static BootstrapContext Initialize()
    {
        var httpClient = new HttpClient();
        IRandomUserClient randomUserClient = new RandomUserHttpClient(httpClient);
        IScheduleRepository repository = new FileScheduleRepository(Path.Combine(AppContext.BaseDirectory, "data", "schedules"));

        var strategies = new Dictionary<string, ISchedulingStrategy>(StringComparer.OrdinalIgnoreCase)
        {
            ["round-robin"] = new RoundRobinStrategy(),
            ["fairness"] = new FairnessStrategy()
        };

        var scheduler = new Scheduler();
        var consoleWriter = new SystemConsoleWriter();

        return new BootstrapContext(randomUserClient, repository, strategies, scheduler, consoleWriter);
    }
}
