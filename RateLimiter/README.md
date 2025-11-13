# Rate Limiter asincrono – SemaphoreSlim e Task

Questo mini–progetto dimostrativo mostra come realizzare, passo dopo passo, un rate limiter asincrono che regola quante richieste possono essere elaborate in parallelo e quante possono essere accettate in una determinata finestra temporale. L'applicazione console fornisce uno scheletro completo, già compilabile, che guida lo studente nell'implementazione della logica principale tramite TODO distribuiti su più milestone.

L'obiettivo è simulare un servizio che riceve molte richieste "lente" e necessita di un controllo rigoroso del throughput: da un lato limitiamo la concorrenza mediante `SemaphoreSlim`, dall'altro monitoriamo quante richieste vengono accettate in un intervallo temporale scorrevole. Il progetto è pensato per un pubblico con conoscenze intermedie/avanzate di C# e programmazione asincrona.

## Obiettivi didattici

Il percorso guidato tocca i seguenti aspetti fondamentali:

- Comprendere la differenza tra limitare la concorrenza (numero di richieste simultanee) e controllare il throughput (richieste per unità di tempo).
- Utilizzare `SemaphoreSlim` come meccanismo di sincronizzazione asincrona compatibile con `await`.
- Lavorare con `Task`, `async/await` e `Task.WhenAll` per eseguire batch di richieste in parallelo.
- Implementare un semplice rate limiting a finestra temporale mantenendo in memoria i timestamp delle richieste accettate.
- Ragionare sull'uso di `CancellationToken` e sulla propagazione dell'annullamento verso metodi asincroni.

## Requisiti tecnici

- **Framework target**: .NET 8.0.
- **Tipo di progetto**: console application.
- **Compilazione**: dalla cartella `RateLimiter`, eseguire `dotnet build`.
- **Esecuzione**: dalla cartella `RateLimiter`, eseguire `dotnet run --project src/RateLimiter.App/RateLimiter.App.csproj`.

> Nota: il repository contiene lo scheletro di progetto; le principali funzionalità sono lasciate come esercizio allo studente. Tuttavia, il codice fornito è già compilabile grazie a implementazioni placeholder.

## Struttura del progetto

```
RateLimiter/
├── RateLimiter.sln
├── README.md
└── src/
    └── RateLimiter.App/
        ├── RateLimiter.App.csproj
        ├── Program.cs
        ├── Models/
        │   ├── RateLimiterOptions.cs
        │   ├── RequestResult.cs
        │   └── SimulatedRequest.cs
        ├── Services/
        │   ├── RateLimiter.cs
        │   └── RequestSimulator.cs
        └── Utilities/
            └── ConsoleRenderer.cs
```

- **Program.cs**: entry point asincrono dell'applicazione. Configura i servizi e avvia la simulazione.
- **Models**: contiene i DTO utilizzati per rappresentare opzioni, richieste e risultati.
- **Services**: include il rate limiter vero e proprio e il simulatore di richieste.
- **Utilities**: componenti di supporto per l'output a console.

## Concetti teorici chiave

### Rate limiting

Il rate limiting impone un tetto massimo alle richieste accettate in un certo intervallo di tempo. Immaginiamo un'API pubblica che consente fino a 100 richieste al minuto: superata tale soglia, le chiamate successive devono essere rifiutate o rimandate. Nel nostro scenario il limite è completamente in memoria, ma i concetti si applicano anche a sistemi distribuiti.

### SemaphoreSlim

`SemaphoreSlim` è una versione leggera e asincrona di un semaforo. A differenza di `lock`, supporta la programmazione asincrona perché mette a disposizione `WaitAsync`. È ideale per limitare l'accesso a una risorsa condivisa quando il lavoro interno usa `await`.

Esempio minimo:

```csharp
await _semaphore.WaitAsync(cancellationToken);
try
{
    // Sezione critica, massimo N invocazioni contemporanee
}
finally
{
    _semaphore.Release();
}
```

### Task e async/await

I `Task` rappresentano operazioni asincrone. Con `async/await` possiamo scrivere codice asincrono in modo lineare senza bloccare thread. `Task.WhenAll` consente di attendere il completamento di più operazioni in parallelo e raccoglierne i risultati.

### Finestra scorrevole

Per implementare un limite di richieste per finestra temporale, una tecnica semplice è conservare i timestamp delle richieste accettate in una `Queue<DateTime>`. A ogni nuova richiesta:

1. Si eliminano gli elementi più vecchi di `WindowLength`.
2. Se il numero di timestamp rimanenti è inferiore a `MaxRequestsPerWindow`, la richiesta può essere accettata.
3. In caso contrario viene rifiutata.

## Milestones guidate

Ogni milestone introduce un concetto chiave e indica i TODO da completare.

### Milestone 1 – Setup, modelli e batch di richieste

- File interessati: `Services/RequestSimulator.cs`, `Program.cs`.
- Obiettivo: generare un batch di richieste e avviare `Task` concorrenti.
- TODO principali:
  - Creare gli oggetti `SimulatedRequest` con id incrementale.
  - Invocare `RateLimiter.ExecuteAsync` usando `Task.Run` oppure direttamente `Task.WhenAll`.
  - Collegare un `CancellationToken` reale dal `Program`.

### Milestone 2 – Limite di concorrenza con SemaphoreSlim

- File interessato: `Services/RateLimiter.cs`.
- Obiettivo: controllare quante richieste possono lavorare contemporaneamente.
- TODO principali:
  - Chiamare `_semaphore.WaitAsync(cancellationToken)` prima di eseguire il lavoro.
  - Rilasciare il semaforo in `finally` e registrare `StartTime`/`EndTime`.
  - Gestire eventuali eccezioni legate al semaforo e al token di cancellazione.

### Milestone 3 – Limite per finestra temporale

- File interessato: `Services/RateLimiter.cs`.
- Obiettivo: applicare un limite alle richieste accettate in una finestra scorrevole.
- TODO principali:
  - Pulire i timestamp più vecchi dalla coda `_acceptedTimestamps`.
  - Accettare o rifiutare la richiesta in base al conteggio corrente.
  - Restituire un `RequestResult` coerente con l'esito.

### Milestone 4 – Gestione CancellationToken e timeout

- File interessati: `Program.cs`, `Services/RateLimiter.cs`, `Services/RequestSimulator.cs`.
- Obiettivo: supportare l'annullamento della simulazione.
- TODO principali:
  - Collegare `Console.CancelKeyPress` a `CancellationTokenSource`.
  - Propagare `cts.Token` ai metodi e gestire `OperationCanceledException`.
  - Valutare l'uso di `RateLimiterOptions.GlobalTimeout` come estensione.

### Milestone 5 – Statistiche e miglioramento UI

- File interessati: `Utilities/ConsoleRenderer.cs`.
- Obiettivo: analizzare i risultati e migliorare l'output.
- TODO principali:
  - Arricchire il riepilogo con ulteriori statistiche.
  - Migliorare la formattazione (colori, allineamento tabellare, ecc.).

## Esempi d'uso

L'applicazione console, una volta completati i TODO, può essere eseguita come segue:

```bash
cd RateLimiter
 dotnet run --project src/RateLimiter.App/RateLimiter.App.csproj
```

Durante l'esecuzione verrà richiesto il numero di richieste da simulare e se si desidera visualizzare il dettaglio per ciascuna richiesta. Un possibile output, a titolo illustrativo, potrebbe essere:

```
================ CONFIGURAZIONE ================
MaxConcurrentRequests: 3
MaxRequestsPerWindow: 10
WindowLength: 1000 ms
SimulatedWorkDuration: 400 ms
GlobalTimeout: 00:00:10

Quante richieste simulare? (default 25): 25
Mostrare risultati dettagliati? (s/N): s
...
================ RISULTATI DETTAGLIATI ================
Richiesta 001 | Accettata: True | Outcome: Completata | Durata: 401 ms
Richiesta 002 | Accettata: False | Outcome: Rifiutata per limite finestra | Durata: 0 ms
...
================ RIEPILOGO ================
Richieste totali: 25
Accettate: 18
Rifiutate: 7
Durata media richieste accettate: 412.5 ms
```

I valori reali dipenderanno dalla tua implementazione e dai parametri di configurazione.

## Come estendere il progetto

- Sostituire la simulazione basata su `Task.Delay` con vere chiamate HTTP o operazioni I/O.
- Implementare algoritmi di rate limiting alternativi (token bucket, leaky bucket, sliding window logaritmico).
- Aggiungere log su file o integrazione con sistemi di osservabilità.
- Creare test automatici con `xUnit` o `NUnit` che verificano il rispetto dei limiti.
- Introdurre metriche aggiuntive come percentili di latenza.

## Errori comuni

- Dimenticare di chiamare `_semaphore.Release()` nel blocco `finally`, causando deadlock e blocchi.
- Usare `Task.Wait()` o `.Result` invece di `await`, bloccando i thread del thread pool.
- Ignorare il `CancellationToken`, impedendo l'interruzione gentile delle operazioni.
- Applicare il controllo della finestra temporale dopo aver acquisito il semaforo, sprecando slot di concorrenza.
- Non proteggere con lock l'accesso alle strutture condivise (es. la coda di timestamp), portando a race condition.

Buon divertimento con le esercitazioni e buon approfondimento sulla programmazione asincrona in C#!
