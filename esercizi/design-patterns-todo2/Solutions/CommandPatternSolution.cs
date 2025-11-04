using System.Collections.Generic;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del pattern Command con undo/redo e nuovi comandi.
/// </summary>
public static class CommandPatternSolution
{
    public static void Run()
    {
        var editor = new TextEditorReceiver();
        var invoker = new CommandInvoker();

        invoker.Enqueue(new AppendTextCommand(editor, "Ciao"));
        invoker.Enqueue(new AppendTextCommand(editor, " mondo"));
        invoker.Enqueue(new UppercaseCommand(editor));
        invoker.Enqueue(new ClearCommand(editor));

        invoker.ProcessQueue();
        Console.WriteLine($"Risultato finale: '{editor.Content}'");

        invoker.UndoLast();
        Console.WriteLine($"Dopo undo: '{editor.Content}'");

        invoker.RedoLast();
        Console.WriteLine($"Dopo redo: '{editor.Content}'");
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
    private readonly Stack<ICommand> _redo = new();

    public void Enqueue(ICommand command) => _queue.Enqueue(command);

    public void ProcessQueue()
    {
        while (_queue.Count > 0)
        {
            var command = _queue.Dequeue();
            command.Execute();
            _history.Push(command);
            _redo.Clear();
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
        command.Undo();
        _redo.Push(command);
    }

    public void RedoLast()
    {
        if (_redo.Count == 0)
        {
            Console.WriteLine("Nessun comando da ripetere.");
            return;
        }

        var command = _redo.Pop();
        command.Execute();
        _history.Push(command);
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

    public void Clear()
    {
        Console.WriteLine("Clear contenuto editor.");
        Content = string.Empty;
    }
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

        // Ripristiniamo lo stato precedente catturato nell'esecuzione.
        Console.WriteLine("Ripristino stato precedente dopo Uppercase.");
        _receiver.Clear();
        _receiver.Append(_previous);
    }
}

public sealed class ClearCommand : ICommand
{
    private readonly TextEditorReceiver _receiver;
    private string? _previous;

    public ClearCommand(TextEditorReceiver receiver) => _receiver = receiver;

    public void Execute()
    {
        _previous = _receiver.Content;
        _receiver.Clear();
    }

    public void Undo()
    {
        Console.WriteLine("Ripristino contenuto dopo Clear.");
        if (!string.IsNullOrEmpty(_previous))
        {
            _receiver.Append(_previous);
        }
    }
}
