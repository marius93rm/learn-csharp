using System.Collections.Generic;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del Factory Method con report specializzati.
/// </summary>
public static class FactoryMethodPatternSolution
{
    public static void Run()
    {
        Console.WriteLine("Richiesta di report giornaliero: ");
        ReportGenerator generator = new DailyReportGenerator();
        generator.Generate();

        Console.WriteLine();

        Console.WriteLine("Richiesta di report settimanale: ");
        generator = new WeeklyReportGenerator();
        generator.Generate();
    }
}

public abstract class ReportGenerator
{
    public void Generate()
    {
        Console.WriteLine("Preparazione dati...");
        var report = CreateReport();
        report.Header = $"Report generato il {DateTime.Now:d}";
        report.Body = "Contenuto standard";
        report.Footer = "-- fine report --";
        report.AddSection("Statistiche", "Attività completate: 5\nAttività pendenti: 2");
        report.AddSection("Note", "Ricordati di verificare gli indicatori KPI.");
        report.Print();
    }

    protected abstract Report CreateReport();
}

public sealed class DailyReportGenerator : ReportGenerator
{
    protected override Report CreateReport()
    {
        return new PlainTextReport("Report Giornaliero");
    }
}

public sealed class WeeklyReportGenerator : ReportGenerator
{
    protected override Report CreateReport()
    {
        return new MarkdownReport("Report Settimanale");
    }
}

public abstract class Report
{
    protected Report(string title) => Title = title;

    public string Title { get; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Footer { get; set; } = string.Empty;
    protected List<(string Title, string Content)> Sections { get; } = new();

    public void AddSection(string title, string content)
    {
        Sections.Add((title, content));
    }

    public virtual void Print()
    {
        Console.WriteLine(Title);
        Console.WriteLine(Header);
        Console.WriteLine(Body);
        foreach (var (sectionTitle, content) in Sections)
        {
            Console.WriteLine($"[{sectionTitle}]\n{content}");
        }
        Console.WriteLine(Footer);
    }
}

public sealed class PlainTextReport : Report
{
    public PlainTextReport(string title) : base(title)
    {
    }

    public override void Print()
    {
        Console.WriteLine(new string('=', Title.Length));
        base.Print();
        Console.WriteLine(new string('=', Title.Length));
    }
}

public sealed class MarkdownReport : Report
{
    public MarkdownReport(string title) : base(title)
    {
    }

    public override void Print()
    {
        Console.WriteLine($"# {Title}");
        Console.WriteLine($"> {Header}");
        Console.WriteLine();
        Console.WriteLine(Body);
        foreach (var (sectionTitle, content) in Sections)
        {
            Console.WriteLine($"## {sectionTitle}");
            Console.WriteLine(content);
            Console.WriteLine();
        }
        Console.WriteLine(Footer);
    }
}
