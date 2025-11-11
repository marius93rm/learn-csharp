# SalutoUtente (esercizio guidato)

Questo laboratorio ti guida nella creazione di un'app .NET MAUI che chiede all'utente il nome e mostra un saluto personalizzato. L'interfaccia dovrà essere costruita interamente in C#, senza XAML, per mettere in evidenza input, gestione eventi e aggiornamento dinamico della UI.

> 💡 La soluzione completa è disponibile nella cartella [`Soluzione`](Soluzione/) se desideri confrontare il tuo lavoro a esercizio terminato.

## Obiettivo dell'app
- Mostrare un campo di testo per l'inserimento del nome.
- Aggiungere un pulsante che, al clic, genera il saluto "Ciao, [nome]".
- Gestire il caso in cui l'utente non inserisce nulla, fornendo un messaggio di guida.

## Requisiti tecnici
- .NET 8 (o versione compatibile) con workload **.NET MAUI** installato.
- Visual Studio 2022 (Windows o Mac) oppure CLI `dotnet`.
- Emulatore o dispositivo configurato per testare la piattaforma scelta.

## Milestone e passaggi
Segui i passaggi nell'ordine indicato. Ogni milestone contiene spiegazioni e snippet di codice utili; copiali e adattano al tuo progetto.

### Milestone 0 – Creazione della soluzione MAUI
1. Crea una nuova soluzione MAUI chiamata `SalutoUtente`.
   - CLI: `dotnet new maui -n SalutoUtente`
   - Oppure usa Visual Studio → **Create a new project** → `.NET MAUI App` → nome `SalutoUtente`.
2. Apri la soluzione e rimuovi i file XAML (`App.xaml`, `MainPage.xaml` e relative partial class) perché costruiremo l'interfaccia solo in C#.

### Milestone 1 – Configurazione del progetto
1. Apri `SalutoUtente.csproj` e sostituisci gli ItemGroup predefiniti con il seguente, per includere solo i file C# necessari:

   ```xml
   <ItemGroup>
     <Compile Include="App.xaml.cs" />
     <Compile Include="MainPage.cs" />
   </ItemGroup>
   ```

2. Assicurati che il `RootNamespace` sia `SalutoUtente` e che la proprietà `<UseMaui>` sia impostata su `true`.

### Milestone 2 – Creazione dell'entry point dell'app
1. Crea (o modifica) il file `App.xaml.cs` eliminando l'attributo `partial` e implementando la classe `Application` in modo procedurale.
2. Incolla e analizza lo snippet seguente:

   ```csharp
   using Microsoft.Maui;
   using Microsoft.Maui.Controls;

   namespace SalutoUtente;

   public class App : Application
   {
       public App()
       {
           /* Milestone 4: Impostazione della pagina principale dell'app */
           MainPage = new NavigationPage(new MainPage())
           {
               BarBackgroundColor = Colors.SteelBlue,
               BarTextColor = Colors.White
           };
       }
   }
   ```

   - Nota il commento milestone: ci servirà per ricordare dove configuriamo la pagina iniziale.

### Milestone 3 – Layout e controlli UI
1. Crea il file `MainPage.cs` nella root del progetto.
2. Usa lo snippet seguente come base e osserva come ogni sezione è evidenziata da un commento multilinea di milestone:

   ```csharp
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
   ```

3. Verifica che i commenti di milestone corrispondano alle sezioni logiche richieste dall'esercizio.

### Milestone 4 – Esecuzione e test
1. Compila il progetto: `dotnet build` oppure `Build` da Visual Studio.
2. Esegui l'app sulla piattaforma desiderata.
3. Inserisci diversi valori nel campo nome per verificare il comportamento con input valido e vuoto.

## Screenshot
<!-- Placeholder: aggiungere screenshot dell'app in esecuzione qui -->

## Approfondimenti suggeriti
- Prova ad aggiungere un reset del campo dopo il saluto.
- Cambia colori e layout per sperimentare con le proprietà dei controlli.
- Estendi l'esempio aggiungendo un `DatePicker` e personalizzando il messaggio con la data.
