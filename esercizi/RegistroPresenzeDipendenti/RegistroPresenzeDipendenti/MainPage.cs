using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using RegistroPresenzeDipendenti.Models;

namespace RegistroPresenzeDipendenti
{
    public class MainPage : ContentPage
    {
        private readonly ObservableCollection<Dipendente> _presenze = new();

        private readonly Entry _nomeEntry;
        private readonly TimePicker _ingressoPicker;
        private readonly TimePicker _uscitaPicker;
        private readonly CollectionView _presenzeCollectionView;

        public MainPage()
        {
            Title = "Registro Presenze Dipendenti";

            // TODO Studente: personalizza il messaggio di benvenuto per l'azienda del tuo corso.
            // Descrivi nello stesso label una frase motivazionale che illustri perché è importante registrare le presenze.

            _nomeEntry = new Entry
            {
                Placeholder = "Nome dipendente",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            /* Milestone 2: Costruzione della UI per inserire dati di un nuovo dipendente */
            _ingressoPicker = new TimePicker
            {
                Time = new TimeSpan(9, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _uscitaPicker = new TimePicker
            {
                Time = new TimeSpan(17, 0, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var aggiungiButton = new Button
            {
                Text = "Aggiungi presenza",
                HorizontalOptions = LayoutOptions.Fill
            };
            aggiungiButton.Clicked += OnAggiungiClicked;

            /* Milestone 3: Aggiunta di CollectionView per mostrare l'elenco presenze */
            _presenzeCollectionView = new CollectionView
            {
                ItemsSource = _presenze,
                SelectionMode = SelectionMode.None,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid
                    {
                        ColumnDefinitions =
                        {
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Auto),
                            new ColumnDefinition(GridLength.Auto),
                            new ColumnDefinition(GridLength.Auto)
                        },
                        ColumnSpacing = 12,
                        Padding = new Thickness(8, 4)
                    };

                    var nomeLabel = new Label
                    {
                        FontAttributes = FontAttributes.Bold
                    };
                    nomeLabel.SetBinding(Label.TextProperty, nameof(Dipendente.Nome));

                    var ingressoLabel = new Label();
                    ingressoLabel.SetBinding(Label.TextProperty, nameof(Dipendente.OraIngresso), stringFormat: "Ingresso: {0:hh\\:mm}");

                    var uscitaLabel = new Label();
                    uscitaLabel.SetBinding(Label.TextProperty, nameof(Dipendente.OraUscita), stringFormat: "Uscita: {0:hh\\:mm}");

                    var totaleLabel = new Label
                    {
                        TextColor = Colors.Green
                    };
                    totaleLabel.SetBinding(Label.TextProperty, nameof(Dipendente.OreTotaliFormattate), stringFormat: "Totale: {0}");

                    grid.Add(nomeLabel, 0, 0);
                    grid.Add(ingressoLabel, 1, 0);
                    grid.Add(uscitaLabel, 2, 0);
                    grid.Add(totaleLabel, 3, 0);

                    return grid;
                })
            };

            var layout = new VerticalStackLayout
            {
                Padding = new Thickness(20),
                Spacing = 16,
                Children =
                {
                    new Label
                    {
                        Text = "Registra le presenze giornaliere dei dipendenti",
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label { Text = "Nome" },
                    _nomeEntry,
                    new Label { Text = "Ora ingresso" },
                    _ingressoPicker,
                    new Label { Text = "Ora uscita" },
                    _uscitaPicker,
                    // TODO Studente: aggiungi qui un controllo opzionale (ad esempio uno Switch)
                    // per indicare se il dipendente è in smart working e mostra l'informazione nella lista.
                    aggiungiButton,
                    new BoxView { HeightRequest = 1, Color = Colors.LightGray },
                    new Label
                    {
                        Text = "Elenco presenze registrate",
                        FontAttributes = FontAttributes.Bold
                    },
                    _presenzeCollectionView
                }
            };

            Content = layout;
        }

        /* Milestone 4: Calcolo automatico delle ore lavorate e visualizzazione */
        private async void OnAggiungiClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nomeEntry.Text))
            {
                await DisplayAlert("Nome mancante", "Inserisci il nome del dipendente.", "OK");
                return;
            }

            if (_uscitaPicker.Time < _ingressoPicker.Time)
            {
                // TODO Studente: gestisci orari notturni se la tua azienda li utilizza.
                // Ad esempio, puoi aggiungere un controllo che consideri un turno oltre la mezzanotte.
                await DisplayAlert("Orario non valido", "L'ora di uscita deve essere successiva a quella di ingresso.", "OK");
                return;
            }

            var presenza = new Dipendente
            {
                Nome = _nomeEntry.Text.Trim(),
                OraIngresso = _ingressoPicker.Time,
                OraUscita = _uscitaPicker.Time
            };

            _presenze.Add(presenza);

            // TODO Studente: collega questo punto ad un servizio REST o ad un database locale.
            // Documenta nel README le scelte architetturali che faresti per sincronizzare i dati con l'ufficio HR.

            _nomeEntry.Text = string.Empty;
            _ingressoPicker.Time = new TimeSpan(9, 0, 0);
            _uscitaPicker.Time = new TimeSpan(17, 0, 0);
        }
    }
}
