# Esercizio TODO – Design Pattern GoF (Seconda Parte)

Benvenuto nel secondo pacchetto di esercizi dedicato ai design pattern del *Gang of Four*. Troverai una soluzione console in C# (.NET 6+) con codice già compilabile ma incompleto. Il tuo compito è risolvere i `// TODO:` presenti in ciascun file per completare l'implementazione dei pattern.

## Prerequisiti

- SDK .NET 6 o superiore installato
- Editor C# (Visual Studio, VS Code + C# Dev Kit, Rider, ecc.)
- Familiarità di base con i pattern GoF e con i concetti di object-oriented programming

## Come iniziare

1. Apri la cartella `esercizi/design-patterns-todo2` nel tuo editor preferito.
2. Leggi `Program.cs` per capire l'ordine con cui vengono eseguite le dimostrazioni.
3. Entra nella cartella `Patterns/` e affronta un file per volta, completando i `// TODO:`.
4. Dopo ogni modifica esegui `dotnet run` per verificare che tutto compili e che l'output rispecchi le tue aspettative.

## Guida ai pattern e attività richieste

Di seguito trovi una descrizione sintetica di ogni pattern e i passi da seguire nel relativo file.

### `SingletonPattern.cs`
- **Descrizione:** gestisce una singola istanza condivisa della configurazione di gioco.
- **Da fare:**
  - Rendere thread-safe l'accesso a `GameConfiguration.Instance` implementando un'inizializzazione pigra corretta.
  - Validare gli input di `Update` e notificare/loggare le modifiche effettuate.
  - Valuta l'aggiunta di proprietà o metodi di utilità per osservare gli effetti del singleton.
- **Suggerimenti:** usa `lock`, `Lazy<T>` o altre tecniche per evitare più istanze in ambienti multi-thread.

### `FactoryMethodPattern.cs`
- **Descrizione:** fornisce un template di generazione report delegando alle sottoclassi la scelta del tipo concreto.
- **Da fare:**
  - Popolare i report con dati specifici nel metodo `Generate`.
  - Restituire report diversi in `DailyReportGenerator` e `WeeklyReportGenerator` (puoi creare nuove classi se necessario).
  - Personalizzare la stampa in `PlainTextReport.Print` per renderla più leggibile.
- **Suggerimenti:** crea sottoclassi di `Report` con stile proprio (Markdown, HTML, ecc.).

### `AbstractFactoryPattern.cs`
- **Descrizione:** crea famiglie di componenti grafici coerenti (tema chiaro/scuro).
- **Da fare:**
  - Coordinare le interazioni tra finestra, bottone e checkbox nel metodo `RenderUi`.
  - Valutare l'introduzione di un nuovo prodotto nell'interfaccia `IGuiFactory` e nelle factory concrete.
  - Implementare comportamenti specifici nei metodi `Click` e `Toggle`, mantenendo consistenza con il tema.
- **Suggerimenti:** puoi far sì che il click del bottone attivi/disattivi la checkbox o cambi il titolo della finestra.

### `CommandPattern.cs`
- **Descrizione:** incapsula operazioni di editing testo in oggetti comando con supporto all'undo.
- **Da fare:**
  - Estendere `CommandInvoker` per supportare il redo opzionale.
  - Aggiungere nuove operazioni nel ricevitore (`TextEditorReceiver`) e creare i relativi comandi.
  - Implementare correttamente l'undo di `UppercaseCommand` ripristinando il testo salvato.
- **Suggerimenti:** usa due stack (undo/redo) e ricordati di resettare il redo quando arriva un nuovo comando.

### `InterpreterPattern.cs`
- **Descrizione:** valuta espressioni booleane e numeriche su un contesto di variabili.
- **Da fare:**
  - Gestire la lettura di variabili mancanti con eccezioni o valori di fallback documentati.
  - Consentire a `ConstantExpression` di interpretare numeri/stringhe oltre ai booleani.
  - Migliorare `GreaterThanExpression` gestendo input non numerici e aggiungere nuove espressioni (es. `NotExpression`).
- **Suggerimenti:** crea metodi di supporto per convertire in modo sicuro i valori e mostrare messaggi esplicativi.

### `StrategyPattern.cs`
- **Descrizione:** consente di scegliere dinamicamente l'algoritmo di calcolo delle spese di spedizione.
- **Da fare:**
  - Arricchire `CartItem` con dati utili (peso, categoria) e usarli nelle strategie.
  - Personalizzare i costi di `StandardShippingStrategy` ed `ExpressShippingStrategy` basandoti su regole realistiche.
  - Introdurre logica di selezione automatica della strategia in base al carrello.
- **Suggerimenti:** puoi creare una factory di strategie che scelga l'implementazione più adatta.

### `DecoratorPattern.cs`
- **Descrizione:** permette di aggiungere responsabilità extra a un sistema di notifiche runtime.
- **Da fare:**
  - Validare indirizzi e numeri nei decoratori esistenti e gestire eventuali errori di invio.
  - Implementare limiti o code di messaggi negli SMS.
  - Creare nuovi decoratori (es. notifiche push, Slack) e provarli nel metodo `Run`.
- **Suggerimenti:** ricorda di chiamare `base.Send` per mantenere la catena e valuta l'ordine dei decoratori.

### `FacadePattern.cs`
- **Descrizione:** espone un'interfaccia unica per prenotare viaggi orchestrando più servizi.
- **Da fare:**
  - Gestire i fallimenti di pagamento annullando le prenotazioni parziali.
  - Simulare scenari alternativi (pagamenti rifiutati, disponibilità terminata) con messaggi chiari.
  - Aggiungere nuovi servizi opzionali (auto, assicurazione) e integrarli nella facciata.
- **Suggerimenti:** usa metodi privati per mantenere il codice della facciata leggibile e coeso.

### `AdapterPattern.cs`
- **Descrizione:** adatta un servizio meteo legacy all'interfaccia moderna `IWeatherProvider`.
- **Da fare:**
  - Gestire errori/dati mancanti nel risultato legacy con fallback o eccezioni personalizzate.
  - Implementare un secondo adapter per un servizio differente e confrontarne l'uso in `Run`.
  - Valutare la creazione di adapter compositi per aggregare dati da più fonti.
- **Suggerimenti:** sfrutta classi record o DTO per trasformare i dati legacy nel formato moderno.

## Prossimi passi

1. Completa i TODO in ordine a tua scelta.
2. Esegui `dotnet run` per verificare che gli output corrispondano alle aspettative.
3. Estendi il progetto con ulteriori pattern o casi d'uso per consolidare la comprensione.

Buon lavoro e buon divertimento con i design pattern!
