# Esempi di programmazione asincrona in C#

Questa cartella contiene un progetto console `.NET 6` pensato per illustrare in modo pratico
le principali tecniche di programmazione asincrona disponibili nella libreria standard.
Tutti gli esempi sono in italiano, ampiamente commentati e raggiungibili da una semplice
interfaccia a riga di comando.

## Requisiti

- [.NET SDK 6.0 o superiore](https://dotnet.microsoft.com/en-us/download).

Puoi verificare di avere la versione corretta eseguendo:

```bash
dotnet --version
```

## Come avviare il progetto

1. Apri un terminale nella radice del repository.
2. Spostati nella cartella del progetto:
   ```bash
   cd esempi/asincrona
   ```
3. Ripristina (se necessario) ed esegui il progetto:
   ```bash
   dotnet run
   ```

All'avvio vedrai un menù numerato: digita il numero dell'esempio che vuoi eseguire (oppure `0`
o `Q` per uscire). Ogni scenario mostra messaggi esplicativi durante l'esecuzione così da poter
osservare il comportamento delle `Task` e del runtime asincrono.

## Struttura del codice

- `Program.cs`: contiene il menù interattivo. È un loop `async` che istanzia e avvia gli esempi
  implementati in `Examples/`.
- `Examples/`: cartella con una classe per ciascun esempio, tutte implementano l'interfaccia
  comune `IAsyncExample`.
- `AsincronaExamples.csproj`: file di progetto .NET (target `net6.0`).

## Panoramica degli esempi

Ogni esempio si concentra su un aspetto specifico della programmazione asincrona:

1. **Async/Await di base** (`BasicAwaitExample`)
   - Simula una chiamata I/O lenta usando `Task.Delay`.
   - Mostra come usare `await` per non bloccare il thread chiamante e misura il tempo impiegato.
2. **Operazioni parallele** (`ParallelRequestsExample`)
   - Esegue tre operazioni in parallelo e raccoglie i risultati con `Task.WhenAll`.
   - Evidenzia come il tempo totale sia pari all'operazione più lunga, non alla somma delle durate.
3. **Cancellazione** (`CancellationExample`)
   - Utilizza `CancellationTokenSource` per interrompere un conteggio.
   - L'utente può premere INVIO per annullare, in alternativa avviene un timeout automatico.
4. **Gestione eccezioni** (`ExceptionHandlingExample`)
   - Dimostra come intercettare eccezioni lanciate da metodi asincroni e preservare lo stack trace.
   - Include un blocco `finally` per evidenziare la corretta gestione delle risorse.
5. **Avanzamento lavori** (`ProgressReportingExample`)
   - Usa `IProgress<T>` e `Progress<T>` per notificare l'avanzamento di un'elaborazione.
   - Aggiorna la console in tempo reale, simulando come si potrebbe aggiornare una UI.

## Estensioni suggerite

- Integrare vere chiamate HTTP con `HttpClient` per confrontare i tempi reali.
- Sostituire la console con una UI grafica (WinUI, WPF, MAUI) riutilizzando gli stessi concetti.
- Aggiungere test automatici che verificano il comportamento delle Task con `xUnit`.

Buono studio e buon divertimento con l'asincronia in C#!
