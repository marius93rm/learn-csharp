using RandomUserShifts.App.Cli;
using RandomUserShifts.App.Composition;
using RandomUserShifts.Domain.Entities;
using RandomUserShifts.Domain.Rules;
using RandomUserShifts.Infrastructure.Mapping;

await ProgramEntrypoint.RunAsync(args);

public static class ProgramEntrypoint
{
    public static async Task RunAsync(string[] args, BootstrapContext? context = null, CancellationToken cancellationToken = default)
    {
        var bootstrap = context ?? Bootstrap.Initialize();
        var parser = new ArgsParser();
        var arguments = parser.Parse(args);

        var peopleDtos = await bootstrap.RandomUserClient.GetRandomUsersAsync(arguments.People, cancellationToken);
        var people = peopleDtos.Select(PersonMapper.Map).ToList();

        if (!bootstrap.Strategies.TryGetValue(arguments.Strategy, out var strategy))
        {
            strategy = bootstrap.Strategies["round-robin"];
        }

        var rules = new SchedulingRules();
        var schedule = bootstrap.Scheduler.Build(arguments.WeekStart, people, rules, strategy);
        await bootstrap.ScheduleRepository.SaveAsync(schedule, cancellationToken);

        bootstrap.ConsoleWriter.WriteLine($"RandomUser Shifts — Week {schedule.WeekStartDate:yyyy-MM-dd}");
        // TODO[7]: stampa un riepilogo leggibile: Giorno -> [HH:MM-HH:MM] Persona; e conteggi per persona.
        bootstrap.ConsoleWriter.WriteLine("Schedule generation is pending TODO completion.");
    }
}
