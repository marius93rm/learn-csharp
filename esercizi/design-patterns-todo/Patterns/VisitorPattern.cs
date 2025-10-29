namespace DesignPatternsTodo.Patterns;

/// <summary>
/// Esempio didattico del pattern Visitor.
/// Completa i TODO per aggiungere nuove operazioni sugli elementi visitabili.
/// </summary>
public static class VisitorPattern
{
    public static void Run()
    {
        var elements = new IElement[]
        {
            new Paragraph("Il pattern Visitor permette di aggiungere operazioni senza modificare le classi"),
            new ImageElement("diagramma.png")
        };

        IVisitor renderVisitor = new RenderVisitor();
        foreach (var element in elements)
        {
            element.Accept(renderVisitor);
        }

        Console.WriteLine("\nAggiungi nuovi visitor o nuovi elementi completando i TODO.\n");
    }

    private interface IVisitor
    {
        void VisitParagraph(Paragraph paragraph);
        void VisitImage(ImageElement image);
        // TODO: aggiungi qui i metodi visit per eventuali nuovi elementi (es. Tabella).
    }

    private interface IElement
    {
        void Accept(IVisitor visitor);
    }

    private sealed class Paragraph : IElement
    {
        public Paragraph(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitParagraph(this);
        }
    }

    private sealed class ImageElement : IElement
    {
        public ImageElement(string source)
        {
            Source = source;
        }

        public string Source { get; }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitImage(this);
        }
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

        // TODO: implementa qui la logica per eventuali nuovi elementi visitabili.
    }
}
