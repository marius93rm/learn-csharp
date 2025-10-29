# Esercizio TODO – Design Pattern GoF

Questo esercizio propone una serie di esempi didattici sui principali design pattern del *Gang of Four*. Ogni pattern è rappresentato da un file C# contenente codice funzionante ma incompleto: tocca a te completare i `// TODO:` per sperimentare l'implementazione.

## Requisiti

- .NET 6 o superiore
- Ambiente in grado di compilare ed eseguire un progetto console C#

## Come iniziare

1. Apri la soluzione nella cartella `esercizi/design-patterns-todo`.
2. Esamina `Program.cs` per vedere come vengono eseguite le dimostrazioni.
3. Procedi pattern per pattern, seguendo le indicazioni qui sotto.
4. Dopo ogni modifica, esegui `dotnet run` per osservare il nuovo comportamento.

## Guida passo passo ai pattern

### Prototype – `Patterns/PrototypePattern.cs`
- **Idea:** creare copie di un oggetto senza conoscere la classe concreta, evitando effetti collaterali.
- **Cosa fare:** implementa la copia profonda della collezione `Abilities` nel metodo `Clone`. In questo modo, modificare il clone non altererà l'originale.
- **Suggerimento:** usa il costruttore della `ObservableCollection` o crea una nuova istanza e copia i valori.

### Builder – `Patterns/BuilderPattern.cs`
- **Idea:** separare la costruzione di un oggetto complesso dai suoi singoli passi.
- **Cosa fare:** arricchisci `PlainTextReportBuilder` aggiungendo separatori, numerazioni o altre informazioni nelle sezioni. Ricorda di aggiornare il metodo `Reset` se introduci nuovi campi di stato.
- **Suggerimento:** puoi mantenere un contatore interno per numerare le sezioni.

### Object Pool – `Patterns/ObjectPoolPattern.cs`
- **Idea:** riutilizzare oggetti costosi da creare mantenendoli in un pool.
- **Cosa fare:** implementa la logica di preallocazione nel metodo `Acquire` e decidi come gestire il rilascio di oggetti non appartenenti al pool. Personalizza anche il metodo `Reset` di `Bullet` per azzerare lo stato.
- **Suggerimento:** crea ad esempio più proiettili in anticipo o lancia un'eccezione quando qualcuno rilascia un oggetto estraneo.

### Bridge – `Patterns/BridgePattern.cs`
- **Idea:** separare un'astrazione dalla sua implementazione in modo che possano variare indipendentemente.
- **Cosa fare:** aggiungi un metodo virtuale/astratto in `RemoteControl` (es. gestione volume) e fornisci la relativa implementazione in `BasicRemote`. Puoi anche creare nuovi dispositivi.
- **Suggerimento:** estendi `IDevice` con proprietà e metodi aggiuntivi se ti servono per il nuovo controllo.

### Composite – `Patterns/CompositePattern.cs`
- **Idea:** trattare oggetti singoli e composti in maniera uniforme.
- **Cosa fare:** aggiungi metodi come `Remove`, `Count` o ricerca all'interfaccia `INode` e implementali in `Folder`. Decidi come gestire tali operazioni nei `Document`.
- **Suggerimento:** per il conteggio ricorsivo, somma i risultati dei figli.

### Flyweight – `Patterns/FlyweightPattern.cs`
- **Idea:** condividere lo stato intrinseco tra molti oggetti leggeri per ridurre l'uso di memoria.
- **Cosa fare:** aggiungi un metodo per svuotare/limitare la cache nel `TreeFactory` e arricchisci `TreeType` con dati intrinseci (es. texture). Valuta come aggiornare la chiave di caching.
- **Suggerimento:** una semplice `ClearCache` che svuota il dizionario è un buon punto di partenza.

### Proxy – `Patterns/ProxyPattern.cs`
- **Idea:** sostituire l'oggetto reale con uno “surrogato” che controlla l'accesso.
- **Cosa fare:** implementa controlli di autorizzazione o logging prima di creare `HeavyImage`, e aggiungi logiche post-accesso (es. contatore di visualizzazioni).
- **Suggerimento:** puoi salvare in una variabile il numero di volte in cui l'immagine è stata mostrata.

### Chain of Responsibility – `Patterns/ChainOfResponsibilityPattern.cs`
- **Idea:** incatenare gestori che possono elaborare o passare avanti una richiesta.
- **Cosa fare:** amplia `LevelOneSupportHandler` con altri casi semplici e decidi cosa fare nel `ManagerSupportHandler`. Puoi introdurre nuovi handler o cambiare l'ordine della catena.
- **Suggerimento:** crea un nuovo handler per richieste relative a “hardware” e inseriscilo nella catena.

### Iterator – `Patterns/IteratorPattern.cs`
- **Idea:** fornire un modo uniforme per attraversare gli elementi di una collezione senza esporre la rappresentazione interna.
- **Cosa fare:** aggiungi un iteratore personalizzato (es. inverso o filtrato) e rendilo accessibile tramite un nuovo metodo in `CourseCollection`. Valuta la gestione delle risorse in `Dispose` se necessario.
- **Suggerimento:** implementa una classe `ReverseIterator` che parte dall'ultimo elemento.

### Mediator – `Patterns/MediatorPattern.cs`
- **Idea:** centralizzare le comunicazioni tra oggetti riducendo le dipendenze dirette.
- **Cosa fare:** implementa messaggi privati, filtri o log nella `ChatRoomMediator`. Puoi anche far sì che `ChatUser.Receive` reagisca automaticamente a certi messaggi.
- **Suggerimento:** usa un dizionario per cercare rapidamente un partecipante destinatario.

### Memento – `Patterns/MementoPattern.cs`
- **Idea:** catturare e ripristinare lo stato interno di un oggetto senza violare l'incapsulamento.
- **Cosa fare:** arricchisci `EditorMemento` con metadati e gestisci nella `EditorHistory` limiti o navigazione avanti/indietro.
- **Suggerimento:** aggiungi un timestamp usando `DateTimeOffset`.

### State – `Patterns/StatePattern.cs`
- **Idea:** incapsulare il comportamento legato allo stato corrente di un oggetto.
- **Cosa fare:** aggiungi proprietà contestuali (es. traccia corrente) a `AudioPlayer` e gestiscile nei vari stati. Decidi nuove transizioni (es. stato `Locked`).
- **Suggerimento:** aggiorna lo stato della traccia nel metodo `Next` e mostra informazioni aggiuntive.

### Template Method – `Patterns/TemplateMethodPattern.cs`
- **Idea:** definire la struttura di un algoritmo lasciando alle sottoclassi l'implementazione di alcuni passi.
- **Cosa fare:** consenti alle sottoclassi di aggiungere condimenti in `Serve` e sovrascrivi il metodo in `PastaCooking` per mostrare il risultato finale.
- **Suggerimento:** chiama `base.Serve()` e aggiungi messaggi extra prima o dopo.

### Visitor – `Patterns/VisitorPattern.cs`
- **Idea:** separare un algoritmo dalla struttura dati su cui opera permettendo nuove operazioni senza modificare le classi degli elementi.
- **Cosa fare:** aggiungi nuovi elementi (es. `Table`) e i relativi metodi `Visit`. Amplia `RenderVisitor` o crea nuovi visitor (es. esportazione Markdown).
- **Suggerimento:** ricorda di aggiornare l'interfaccia `IVisitor` quando introduci nuovi elementi.

## Prossimi passi

- Completa i TODO nell'ordine che preferisci.
- Esegui `dotnet run` per verificare l'effetto delle modifiche.
- Sperimenta aggiungendo casi personalizzati per consolidare la comprensione dei pattern.

Buon lavoro!
