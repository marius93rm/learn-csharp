using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesignPatternsTodo.Solutions;

/// <summary>
/// Soluzione Visitor con un nuovo elemento Tabella e un visitor per l'esportazione Markdown.
/// </summary>
public static class VisitorPatternSolution
{
    public static void Run()
    {
        var elements = new IElement[]
        {
            new Paragraph("Il pattern Visitor permette nuove operazioni senza modificare le classi."),
            new ImageElement("diagramma.png"),
            new TableElement(new [] { "Nome", "Valore" }, new [] { "Pattern", "Visitor" })
        };

        IVisitor renderVisitor = new RenderVisitor();
        foreach (var element in elements)
        {
            element.Accept(renderVisitor);
        }

        Console.WriteLine("\nEsportazione Markdown:");
        IVisitor markdownVisitor = new MarkdownExportVisitor();
        foreach (var element in elements)
        {
            element.Accept(markdownVisitor);
        }
    }

    private interface IVisitor
    {
        void VisitParagraph(Paragraph paragraph);
        void VisitImage(ImageElement image);
        void VisitTable(TableElement table);
    }

    private interface IElement
    {
        void Accept(IVisitor visitor);
    }

    private sealed class Paragraph : IElement
    {
        public Paragraph(string text) => Text = text;

        public string Text { get; }

        public void Accept(IVisitor visitor) => visitor.VisitParagraph(this);
    }

    private sealed class ImageElement : IElement
    {
        public ImageElement(string source) => Source = source;

        public string Source { get; }

        public void Accept(IVisitor visitor) => visitor.VisitImage(this);
    }

    private sealed class TableElement : IElement
    {
        public TableElement(IReadOnlyList<string> headers, IReadOnlyList<string> values)
        {
            Headers = headers;
            Values = values;
        }

        public IReadOnlyList<string> Headers { get; }
        public IReadOnlyList<string> Values { get; }

        public void Accept(IVisitor visitor) => visitor.VisitTable(this);
    }

    private sealed class RenderVisitor : IVisitor
    {
        public void VisitParagraph(Paragraph paragraph)
        {
            Console.WriteLine($"[Testo] {paragraph.Text}");
        }

        public void VisitImage(ImageElement image)
        {
            Console.WriteLine($"[Immagine] {image.Source}");
        }

        public void VisitTable(TableElement table)
        {
            Console.WriteLine("[Tabella]");
            for (var i = 0; i < table.Headers.Count && i < table.Values.Count; i++)
            {
                Console.WriteLine($"  {table.Headers[i]}: {table.Values[i]}");
            }
        }
    }

    private sealed class MarkdownExportVisitor : IVisitor
    {
        public void VisitParagraph(Paragraph paragraph)
        {
            Console.WriteLine(paragraph.Text);
            Console.WriteLine();
        }

        public void VisitImage(ImageElement image)
        {
            Console.WriteLine($"![{Path.GetFileNameWithoutExtension(image.Source)}]({image.Source})");
        }

        public void VisitTable(TableElement table)
        {
            var headerRow = string.Join(" | ", table.Headers);
            var separatorRow = string.Join(" | ", table.Headers.Select(_ => "---"));
            var valueRow = string.Join(" | ", table.Values);

            Console.WriteLine(headerRow);
            Console.WriteLine(separatorRow);
            Console.WriteLine(valueRow);
            Console.WriteLine();
        }
    }
}
