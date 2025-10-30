/*
 * Pattern: Factory Method
 * Obiettivi didattici:
 *   - Comprendere come demandare la creazione di oggetti alle sottoclassi.
 *   - Variare il tipo concreto generato senza cambiare il codice client.
 *   - Esporre un flusso di elaborazione comune che usa la factory method.
 * Istruzioni:
 *   - Implementa le parti mancanti della factory e personalizza i report.
 */

namespace DesignPatternsTodo2.Patterns;

public static class FactoryMethodPattern
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
        // TODO: arricchisci il report con informazioni specifiche (es. statistiche, note, ecc.).
        report.Print();
    }

    protected abstract Report CreateReport();
}

public sealed class DailyReportGenerator : ReportGenerator
{
    protected override Report CreateReport()
    {
        // TODO: restituisci una sottoclasse concreta di Report adatta al contesto giornaliero.
        return new PlainTextReport("Report Giornaliero");
    }
}

public sealed class WeeklyReportGenerator : ReportGenerator
{
    protected override Report CreateReport()
    {
        // TODO: crea un report differente, magari con formattazione alternativa o dati aggregati.
        return new PlainTextReport("Report Settimanale");
    }
}

public abstract class Report
{
    protected Report(string title) => Title = title;

    public string Title { get; }
    public string Header { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Footer { get; set; } = "-- fine report --";

    public virtual void Print()
    {
        Console.WriteLine(Title);
        Console.WriteLine(Header);
        Console.WriteLine(Body);
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
        // TODO: personalizza la formattazione del testo (es. separatori, maiuscole, ecc.).
        base.Print();
    }
}
