namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Versione completata del builder testuale.
/// Viene introdotta una numerazione progressiva con separatori leggibili
/// e viene ripulito ogni stato interno nel Reset.
/// </summary>
public static class BuilderPatternSolution
{
    public static void Run()
    {
        var director = new ReportDirector(new PlainTextReportBuilder());
        var report = director.BuildSimpleDailyReport();
        Console.WriteLine(report);
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
        private int _sectionCounter;

        public void AddTitle(string title)
        {
            _parts.Add($"*** {title.ToUpperInvariant()} ***");
        }

        public void AddSection(string heading, string body)
        {
            _sectionCounter++;
            // Numeriamo le sezioni e inseriamo un separatore per facilitarne la lettura.
            _parts.Add($"--- Sezione {_sectionCounter}: {heading} ---\n{body}");
        }

        public string Build()
        {
            var content = string.Join("\n\n", _parts);
            Reset();
            return content;
        }

        private void Reset()
        {
            // Oltre a svuotare la lista, riportiamo il contatore al valore iniziale.
            _parts.Clear();
            _sectionCounter = 0;
        }
    }

    private sealed class ReportDirector
    {
        private readonly IReportBuilder _builder;

        public ReportDirector(IReportBuilder builder) => _builder = builder;

        public string BuildSimpleDailyReport()
        {
            _builder.AddTitle("Report quotidiano");
            _builder.AddSection("Attività", "- Pianificazione lezioni\n- Risoluzione esercizi");
            _builder.AddSection("Note", "Ricordati di completare i TODO per migliorare il report.");
            return _builder.Build();
        }
    }
}
