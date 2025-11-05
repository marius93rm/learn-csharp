namespace RandomUserShifts.App.Cli;

public sealed record AppArguments(int People, DateOnly WeekStart, string Strategy);

public sealed class ArgsParser
{
    private const int DefaultPeople = 5;
    private const string DefaultStrategy = "round-robin";

    public AppArguments Parse(string[] args)
    {
        var people = DefaultPeople;
        var weekStart = DateOnly.FromDateTime(DateTime.Today);
        var strategy = DefaultStrategy;

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "--people" && i + 1 < args.Length && int.TryParse(args[i + 1], out var parsedPeople))
            {
                people = parsedPeople;
                i++;
            }
            else if (args[i] == "--week" && i + 1 < args.Length && DateOnly.TryParse(args[i + 1], out var parsedWeek))
            {
                weekStart = parsedWeek;
                i++;
            }
            else if (args[i] == "--strategy" && i + 1 < args.Length)
            {
                strategy = args[i + 1];
                i++;
            }
        }

        // TODO[6]: calcola default weekStart = prossimo lunedì se non specificato.
        return new AppArguments(people, weekStart, strategy);
    }
}
