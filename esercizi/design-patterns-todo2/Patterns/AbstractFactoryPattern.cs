/*
 * Pattern: Abstract Factory
 * Obiettivi didattici:
 *   - Creare famiglie di oggetti correlati senza vincolare il codice client a classi concrete.
 *   - Integrare facilmente nuove varianti (es. tema Scuro/Chiaro).
 *   - Coordinare il comportamento tra i prodotti della stessa famiglia.
 * Istruzioni:
 *   - Completa i TODO per creare una famiglia di controlli coerente.
 */

namespace DesignPatternsTodo2.Patterns;

public static class AbstractFactoryPattern
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

        window.Draw();
        button.Draw();
        checkbox.Draw();

        // TODO: aggiungi interazioni tra i componenti (es. click che cambia tema o testo).
    }
}

public interface IGuiFactory
{
    IWindow CreateWindow();
    IButton CreatePrimaryButton();
    ICheckbox CreateCheckbox();
    // TODO: valuta se aggiungere un nuovo prodotto coerente con il tema (es. menù, toggle, ecc.).
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

public interface ICheckbox
{
    bool IsChecked { get; }
    void Draw();
    void Toggle();
}

public sealed class LightThemeFactory : IGuiFactory
{
    public IWindow CreateWindow() => new LightWindow();
    public IButton CreatePrimaryButton() => new LightButton();
    public ICheckbox CreateCheckbox() => new LightCheckbox();
}

public sealed class DarkThemeFactory : IGuiFactory
{
    public IWindow CreateWindow() => new DarkWindow();
    public IButton CreatePrimaryButton() => new DarkButton();
    public ICheckbox CreateCheckbox() => new DarkCheckbox();
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

internal sealed class LightButton : IButton
{
    public string Caption { get; private set; } = "Conferma";

    public void Draw() => Console.WriteLine($"[Light Button] {Caption}");

    public void Click()
    {
        Console.WriteLine("Click su bottone chiaro");
        // TODO: definisci un'azione coerente col tema (es. mostrare un messaggio colorato).
    }
}

internal sealed class DarkButton : IButton
{
    public string Caption { get; private set; } = "Salva";

    public void Draw() => Console.WriteLine($"[Dark Button] {Caption}");

    public void Click()
    {
        Console.WriteLine("Click su bottone scuro");
        // TODO: coordina l'azione col resto dell'interfaccia (es. cambiare stato della checkbox scura).
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
        // TODO: valuta un side-effect aggiuntivo (es. log in console dedicata al tema scuro).
    }
}
