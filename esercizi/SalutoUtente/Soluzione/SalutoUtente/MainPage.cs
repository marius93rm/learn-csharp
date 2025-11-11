using Microsoft.Maui.Controls;

namespace SalutoUtente;

public class MainPage : ContentPage
{
    private readonly Entry _nameEntry;
    private readonly Label _greetingLabel;

    public MainPage()
    {
        Title = "Saluto Utente";

        /* Milestone 1: Creazione layout con StackLayout */
        var layout = new VerticalStackLayout
        {
            Spacing = 16,
            Padding = new Thickness(24),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        /* Milestone 2: Inserimento controlli per input e feedback */
        var instructionLabel = new Label
        {
            Text = "Inserisci il tuo nome qui sotto",
            FontSize = 20,
            HorizontalTextAlignment = TextAlignment.Center
        };

        _nameEntry = new Entry
        {
            Placeholder = "Il tuo nome",
            HorizontalOptions = LayoutOptions.Fill,
            ClearButtonVisibility = ClearButtonVisibility.WhileEditing
        };

        var greetButton = new Button
        {
            Text = "Salutami",
            HorizontalOptions = LayoutOptions.Center
        };

        _greetingLabel = new Label
        {
            Text = "In attesa del saluto...",
            FontAttributes = FontAttributes.Italic,
            HorizontalTextAlignment = TextAlignment.Center
        };

        layout.Children.Add(instructionLabel);
        layout.Children.Add(_nameEntry);
        layout.Children.Add(greetButton);
        layout.Children.Add(_greetingLabel);

        /* Milestone 3: Gestione evento Click e aggiornamento della UI */
        greetButton.Clicked += OnGreetButtonClicked;

        Content = layout;
    }

    private void OnGreetButtonClicked(object? sender, EventArgs e)
    {
        var name = _nameEntry.Text;
        if (string.IsNullOrWhiteSpace(name))
        {
            _greetingLabel.Text = "Per favore, inserisci il tuo nome.";
            _greetingLabel.FontAttributes = FontAttributes.Italic;
            return;
        }

        _greetingLabel.Text = $"Ciao, {name.Trim()}!";
        _greetingLabel.FontAttributes = FontAttributes.Bold;
    }
}
