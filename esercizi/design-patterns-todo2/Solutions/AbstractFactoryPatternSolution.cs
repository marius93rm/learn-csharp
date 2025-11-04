using System.Collections.Generic;

namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione dell'Abstract Factory con interazioni coordinate tra i componenti.
/// </summary>
public static class AbstractFactoryPatternSolution
{
    public static void Run()
    {
        RenderUi(new LightThemeFactory());
        Console.WriteLine();
        RenderUi(new DarkThemeFactory());
    }

    private static void RenderUi(IGuiFactory factory)
    {
        Console.WriteLine($"Rendering UI con factory: {factory.GetType().Name}");
        var window = factory.CreateWindow();
        var button = factory.CreatePrimaryButton();
        var checkbox = factory.CreateCheckbox();
        var statusBar = factory.CreateStatusBar();

        if (button is IInteractiveButton interactiveButton)
        {
            interactiveButton.Configure(checkbox, statusBar);
        }

        window.Draw();
        button.Draw();
        checkbox.Draw();
        statusBar.Draw();

        button.Click();
        checkbox.Toggle();
        statusBar.UpdateMessage("UI pronta all'uso.");
        statusBar.Draw();
    }
}

public interface IGuiFactory
{
    IWindow CreateWindow();
    IButton CreatePrimaryButton();
    ICheckbox CreateCheckbox();
    IStatusBar CreateStatusBar();
}

public interface IWindow
{
    string Title { get; }
    void Draw();
}

public interface IButton
{
    string Caption { get; }
    void Draw();
    void Click();
}

public interface IInteractiveButton : IButton
{
    void Configure(ICheckbox checkbox, IStatusBar statusBar);
}

public interface ICheckbox
{
    bool IsChecked { get; }
    void Draw();
    void Toggle();
}

public interface IStatusBar
{
    string Message { get; }
    void UpdateMessage(string message);
    void Draw();
}

public sealed class LightThemeFactory : IGuiFactory
{
    public IWindow CreateWindow() => new LightWindow();
    public IButton CreatePrimaryButton() => new LightButton();
    public ICheckbox CreateCheckbox() => new LightCheckbox();
    public IStatusBar CreateStatusBar() => new LightStatusBar();
}

public sealed class DarkThemeFactory : IGuiFactory
{
    public IWindow CreateWindow() => new DarkWindow();
    public IButton CreatePrimaryButton() => new DarkButton();
    public ICheckbox CreateCheckbox() => new DarkCheckbox();
    public IStatusBar CreateStatusBar() => new DarkStatusBar();
}

internal sealed class LightWindow : IWindow
{
    public string Title { get; private set; } = "Finestra Chiara";

    public void Draw() => Console.WriteLine($"[Light] {Title}");
}

internal sealed class DarkWindow : IWindow
{
    public string Title { get; private set; } = "Finestra Scura";

    public void Draw() => Console.WriteLine($"[Dark] {Title}");
}

internal sealed class LightButton : IInteractiveButton
{
    private IStatusBar? _statusBar;

    public string Caption { get; private set; } = "Conferma";

    public void Configure(ICheckbox checkbox, IStatusBar statusBar)
    {
        _statusBar = statusBar;
    }

    public void Draw() => Console.WriteLine($"[Light Button] {Caption}");

    public void Click()
    {
        Console.WriteLine("Click su bottone chiaro: mostro un messaggio di conferma.");
        _statusBar?.UpdateMessage("Operazione completata con successo.");
    }
}

internal sealed class DarkButton : IInteractiveButton
{
    private ICheckbox? _linkedCheckbox;

    public string Caption { get; private set; } = "Salva";

    public void Configure(ICheckbox checkbox, IStatusBar statusBar)
    {
        _linkedCheckbox = checkbox;
        statusBar.UpdateMessage("Premi Salva per confermare le modifiche.");
    }

    public void Draw() => Console.WriteLine($"[Dark Button] {Caption}");

    public void Click()
    {
        Console.WriteLine("Click su bottone scuro: sincronizzo lo stato della checkbox.");
        _linkedCheckbox?.Toggle();
    }
}

internal sealed class LightCheckbox : ICheckbox
{
    public bool IsChecked { get; private set; }

    public void Draw() => Console.WriteLine($"[Light Checkbox] Checked={IsChecked}");

    public void Toggle()
    {
        IsChecked = !IsChecked;
        Console.WriteLine($"Checkbox chiara ora: {IsChecked}");
    }
}

internal sealed class DarkCheckbox : ICheckbox
{
    public bool IsChecked { get; private set; }

    public void Draw() => Console.WriteLine($"[Dark Checkbox] Checked={IsChecked}");

    public void Toggle()
    {
        IsChecked = !IsChecked;
        Console.WriteLine($"Checkbox scura ora: {IsChecked}");
        Console.WriteLine("[Dark Theme] Log: lo stato della checkbox è cambiato.");
    }
}

internal sealed class LightStatusBar : IStatusBar
{
    public string Message { get; private set; } = "Benvenuto";

    public void UpdateMessage(string message)
    {
        Message = message;
    }

    public void Draw()
    {
        Console.WriteLine($"[Light StatusBar] {Message}");
    }
}

internal sealed class DarkStatusBar : IStatusBar
{
    public string Message { get; private set; } = "Modalità notturna attiva";

    public void UpdateMessage(string message)
    {
        Message = message;
    }

    public void Draw()
    {
        Console.WriteLine($"[Dark StatusBar] {Message}");
    }
}
