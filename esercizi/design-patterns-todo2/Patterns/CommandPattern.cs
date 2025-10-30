/*
 * Pattern: Command
 * Obiettivi didattici:
 *   - Incapsulare richieste come oggetti per disaccoppiare invocatore e ricevitore.
 *   - Supportare operazioni annullabili e macro comandi.
 *   - Pianificare l'esecuzione di comandi in una coda.
 * Istruzioni:
 *   - Completa i TODO per aggiungere undo/redo e nuove azioni.
 */

namespace DesignPatternsTodo2.Patterns;

public static class CommandPattern
{
    public static void Run()
    {
        var editor = new TextEditorReceiver();
        var invoker = new CommandInvoker();

        invoker.Enqueue(new AppendTextCommand(editor, "Ciao"));
        invoker.Enqueue(new AppendTextCommand(editor, " mondo"));
        invoker.Enqueue(new UppercaseCommand(editor));

        invoker.ProcessQueue();
        Console.WriteLine($"Risultato finale: '{editor.Content}'");

        // TODO: dimostra la funzionalità di undo/redo una volta implementata.
    }
}

public interface ICommand
{
    void Execute();
    void Undo();
}

public sealed class CommandInvoker
{
    private readonly Queue<ICommand> _queue = new();
    private readonly Stack<ICommand> _history = new();

    public void Enqueue(ICommand command) => _queue.Enqueue(command);

    public void ProcessQueue()
    {
        while (_queue.Count > 0)
        {
            var command = _queue.Dequeue();
            command.Execute();
            _history.Push(command);
        }
    }

    public void UndoLast()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("Nessun comando da annullare.");
            return;
        }

        var command = _history.Pop();
        // TODO: valuta se salvare il comando annullato per implementare il redo.
        command.Undo();
    }
}

public sealed class TextEditorReceiver
{
    public string Content { get; private set; } = string.Empty;

    public void Append(string text)
    {
        Content += text;
        Console.WriteLine($"Append: '{text}' -> {Content}");
    }

    public void RemoveLast(int length)
    {
        if (length <= 0)
        {
            return;
        }

        if (length > Content.Length)
        {
            length = Content.Length;
        }

        Content = Content[..^length];
        Console.WriteLine($"Remove ultimi {length} caratteri -> {Content}");
    }

    public void Uppercase()
    {
        Content = Content.ToUpperInvariant();
        Console.WriteLine($"Uppercase -> {Content}");
    }

    // TODO: aggiungi nuove operazioni del ricevitore (es. Replace, Clear) da collegare a nuovi comandi.
}

public sealed class AppendTextCommand : ICommand
{
    private readonly TextEditorReceiver _receiver;
    private readonly string _text;

    public AppendTextCommand(TextEditorReceiver receiver, string text)
    {
        _receiver = receiver;
        _text = text;
    }

    public void Execute() => _receiver.Append(_text);

    public void Undo() => _receiver.RemoveLast(_text.Length);
}

public sealed class UppercaseCommand : ICommand
{
    private readonly TextEditorReceiver _receiver;
    private string? _previous;

    public UppercaseCommand(TextEditorReceiver receiver) => _receiver = receiver;

    public void Execute()
    {
        _previous = _receiver.Content;
        _receiver.Uppercase();
    }

    public void Undo()
    {
        if (_previous is null)
        {
            Console.WriteLine("Nessun stato precedente salvato per l'undo.");
            return;
        }

        // TODO: ripristina lo stato precedente e gestisci i casi limite.
        _receiver.RemoveLast(0); // istruzione fittizia per mantenere il compilatore soddisfatto
    }
}
