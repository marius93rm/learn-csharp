# Registro Presenze Dipendenti (MAUI)

## Introduzione
Questo esercizio guida passo-passo alla creazione di una semplice app .NET MAUI senza XAML, pensata per registrare le presenze dei dipendenti in un contesto d'ufficio. L'obiettivo è imparare a costruire l'interfaccia utente con il codice C#, utilizzare controlli come `Entry`, `TimePicker` e `CollectionView`, e calcolare le ore lavorate a partire dagli orari di ingresso e uscita.

## Obiettivo dell'app
- Inserire il nome del dipendente e gli orari di ingresso/uscita.
- Salvare i dati in una lista in memoria (nessun database richiesto).
- Mostrare ogni registrazione con le ore totali calcolate automaticamente.

## Struttura dei file
```
esercizi/RegistroPresenzeDipendenti/
├── RegistroPresenzeDipendenti.sln
├── RegistroPresenzeDipendenti/
│   ├── MainPage.cs
│   └── Models/
│       └── Dipendente.cs
└── README.md
```

## Come avviare il progetto in Visual Studio
1. Apri Visual Studio 2022 o successivo con il workload **.NET Multi-platform App UI development** installato.
2. Dal menu `File` seleziona `Open > Project/Solution` e carica `RegistroPresenzeDipendenti.sln`.
3. Imposta l'emulatore o il dispositivo desiderato (Windows, Android, iOS, macOS Catalyst) e premi `F5` per eseguire.
4. La pagina principale `MainPage` verrà creata interamente in C#, quindi non è necessario modificare file XAML.

## Milestone e istruzioni dettagliate
### Milestone 1 – Modello dati `Dipendente`
Dove: `RegistroPresenzeDipendenti/Models/Dipendente.cs`

Scrivi una classe con proprietà per `Nome`, `OraIngresso`, `OraUscita` e una proprietà calcolata per le ore totali. Usa `TimeSpan` per rappresentare gli orari:
```csharp
public class Dipendente
{
    public string Nome { get; set; } = string.Empty;
    public TimeSpan OraIngresso { get; set; }
    public TimeSpan OraUscita { get; set; }
    public TimeSpan OreTotali =>
        OraUscita >= OraIngresso ? OraUscita - OraIngresso : TimeSpan.Zero;
}
```
Test: crea un'istanza nel `MainPage` e verifica che la proprietà `OreTotali` restituisca la differenza corretta.

### Milestone 2 – Interfaccia per l'inserimento dati
Dove: `RegistroPresenzeDipendenti/MainPage.cs`

Costruisci un `VerticalStackLayout` contenente etichette, `Entry` per il nome e `TimePicker` per gli orari. Aggiungi un pulsante "Aggiungi presenza". Esempio di costruzione:
```csharp
_nomeEntry = new Entry { Placeholder = "Nome dipendente" };
_ingressoPicker = new TimePicker { Time = new TimeSpan(9, 0, 0) };
_uscitaPicker = new TimePicker { Time = new TimeSpan(17, 0, 0) };
```
Test: avvia l'app, inserisci nome e orari e premi il pulsante verificando che non si verifichino eccezioni.

### Milestone 3 – Lista presenze con `CollectionView`
Dove: `RegistroPresenzeDipendenti/MainPage.cs`

Crea una `ObservableCollection<Dipendente>` in memoria e collegala a una `CollectionView`. Ogni elemento deve mostrare nome, orario di ingresso, orario di uscita e ore totali formattate.
```csharp
_presenzeCollectionView = new CollectionView
{
    ItemsSource = _presenze,
    ItemTemplate = new DataTemplate(() =>
    {
        // Configura le label e lega le proprietà del modello.
    })
};
```
Test: aggiungi qualche dipendente e verifica che la lista venga aggiornata automaticamente.

### Milestone 4 – Calcolo ore lavorate e reset dei campi
Dove: `RegistroPresenzeDipendenti/MainPage.cs`

Nel gestore del click del pulsante, crea un nuovo `Dipendente`, calcola le ore totali e aggiungilo alla lista. Dopo l'inserimento, svuota i campi per consentire un nuovo inserimento. Ricorda di validare che l'ora di uscita sia successiva a quella di ingresso.
```csharp
private async void OnAggiungiClicked(object? sender, EventArgs e)
{
    if (_uscitaPicker.Time < _ingressoPicker.Time)
    {
        await DisplayAlert("Orario non valido", "L'uscita deve essere dopo l'ingresso.", "OK");
        return;
    }

    _presenze.Add(new Dipendente { ... });
}
```
Test: prova a inserire orari errati per verificare la validazione e controlla che le ore totali vengano calcolate correttamente.

## Suggerimenti aggiuntivi
- Usa i commenti `TODO` in `MainPage.cs` per estendere l'esercizio (es. salvataggio remoto o gestione di turni notturni).
- Personalizza colori e testi per adattare l'app a casi reali del tuo ufficio.
- Puoi creare una pagina di riepilogo settimanale duplicando la `CollectionView` e filtrando per data.

## TODO guidati per lo studente
Per consolidare l'apprendimento, completa le seguenti attività direttamente nel codice seguendo i commenti `TODO Studente` in `MainPage.cs`:

1. **Messaggio di benvenuto personalizzato**  
   - Dove: sezione iniziale del `VerticalStackLayout` in `MainPage.cs`.  
   - Cosa fare: sostituisci il testo della label principale con un messaggio che includa il nome della tua azienda fittizia e un invito motivazionale a timbrare correttamente le presenze.  
   - Come testare: avvia l'app e verifica che il messaggio venga visualizzato correttamente all'apertura.

2. **Indicatore di smart working**  
   - Dove: subito dopo i `TimePicker` in `MainPage.cs`.  
   - Cosa fare: aggiungi un controllo (ad esempio `Switch` o `CheckBox`) per indicare se il dipendente sta lavorando da remoto. Memorizza l'informazione nel modello e mostralo nella `CollectionView` con un'icona o un testo dedicato.  
   - Come testare: inserisci più dipendenti alternando l'opzione smart working e controlla che la lista rifletta la scelta.

3. **Gestione dei turni notturni**  
   - Dove: metodo `OnAggiungiClicked` in `MainPage.cs`.  
   - Cosa fare: modifica la validazione in modo da accettare turni che superano la mezzanotte (es. ingresso alle 22:00 e uscita alle 06:00 del giorno successivo) aggiornando di conseguenza il calcolo delle ore.  
   - Come testare: prova a registrare un turno notturno e verifica che le ore totali risultino corrette.

4. **Preparazione al salvataggio remoto**  
   - Dove: commento `TODO` dopo `_presenze.Add(presenza);`.  
   - Cosa fare: descrivi in un nuovo commento o in una breve documentazione come struttureresti la chiamata verso un servizio REST o un database locale per sincronizzare i dati con l'ufficio HR.  
   - Come testare: non è richiesto un test automatico; verifica però che l'app compili e funzioni ancora dopo l'aggiunta dei commenti e delle eventuali interfacce.

Buon allenamento con MAUI!
