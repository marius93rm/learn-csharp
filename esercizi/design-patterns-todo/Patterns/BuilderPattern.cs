namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Builder.
/// Completa i passaggi marcati TODO per ottenere un report più curato.
/// </summary>
public static class BuilderPattern
{
    public static void Run()
    {
        var director = new ReportDirector(new PlainTextReportBuilder());
        var dailyReport = director.BuildSimpleDailyReport();

        Console.WriteLine(dailyReport);
        Console.WriteLine("\nAggiungi formattazioni e comportamenti extra completando i TODO nel builder.\n");
    }

    private interface IReportBuilder
    {
        void AddTitle(string title);
        void AddSection(string heading, string body);
        string Build();
    }

    private sealed class PlainTextReportBuilder : IReportBuilder
    {
        private readonly List<string> _parts = new();

        public void AddTitle(string title)
        {
            _parts.Add($"*** {title.ToUpperInvariant()} ***");
        }

        public void AddSection(string heading, string body)
        {
            _parts.Add($"[{heading}]\n{body}");
            // TODO: aggiungi qui un elemento di separazione o una numerazione progressiva delle sezioni.
        }

        public string Build()
        {
            var content = string.Join("\n\n", _parts);
            Reset();
            return content;
        }

        private void Reset()
        {
            // TODO: assicurati di ripulire anche eventuali altri campi che aggiungerai.
            _parts.Clear();
        }
    }

    private sealed class ReportDirector
    {
        private readonly IReportBuilder _builder;

        public ReportDirector(IReportBuilder builder)
        {
            _builder = builder;
        }

        public string BuildSimpleDailyReport()
        {
            _builder.AddTitle("Report quotidiano");
            _builder.AddSection("Attività", "- Pianificazione lezioni\n- Risoluzione esercizi");
            _builder.AddSection("Note", "Ricordati di completare i TODO per migliorare il report.");

            return _builder.Build();
        }
    }
}
