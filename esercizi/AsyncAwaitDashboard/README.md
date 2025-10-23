# ⚡ Async Await Dashboard — Esercizio su Task, concorrenza e flussi asincroni

Costruisci un piccolo **dashboard operativo** che aggrega dati da tre servizi simulati:
meteo, quotazioni e diagnostica di sistema. L'esercizio è pensato per lavorare con
`async`/`await`, `Task`, `CancellationToken` e `IAsyncEnumerable`.

---

## 🎯 Obiettivi didattici

* Comprendere come strutturare metodi asincroni che espongono `Task<T>` e `IAsyncEnumerable<T>`.
* Eseguire più chiamate indipendenti **in parallelo** con `Task.WhenAll`.
* Propagare un `CancellationToken` lungo tutto lo stack asincrono.
* Gestire stream di eventi asincroni con `await foreach` e cancellazione.
* Formattare i risultati aggregati in modo leggibile per la console.

---

## 🗂️ Struttura del progetto

```
AsyncAwaitDashboard/
├─ README.md
└─ src/
   └─ AsyncAwaitDashboard.App/
      ├─ AsyncAwaitDashboard.App.csproj
      ├─ Program.cs                 <-- TODO [M5]
      ├─ Dashboard/
      │  ├─ DashboardBuilder.cs     <-- TODO [M3], [M4]
      │  └─ DashboardSnapshot.cs
      ├─ Infrastructure/
      │  ├─ NetworkLatencySimulator.cs  <-- TODO [M1]
      │  └─ SimulatedApiClient.cs
      └─ Providers/
         ├─ DiagnosticsProvider.cs  <-- TODO [M2]
         ├─ IDiagnosticsProvider.cs
         ├─ IQuoteProvider.cs
         ├─ IWeatherProvider.cs
         ├─ QuoteProvider.cs        <-- TODO [M2]
         └─ WeatherProvider.cs      <-- TODO [M2]
```

*(Non sono forniti test automatici: l'obiettivo è concentrarsi sul flusso asincrono. Nulla vieta di aggiungerli!)*

---

## 📚 Ripasso rapido su `async`/`await`

* `Task` rappresenta un'operazione asincrona (eventualmente con valore di ritorno).
* `await` sospende il metodo finché il `Task` non termina senza bloccare il thread.
* `Task.WhenAll` permette di avviare più `Task` e attenderli in parallelo.
* `CancellationToken` va passato a tutte le operazioni che lo supportano per consentire
  la cancellazione cooperativa.
* Gli **stream asincroni** (`IAsyncEnumerable<T>`) si iterano con `await foreach`.

---

## 🧩 Milestone

> Nel codice troverai commenti `// TODO [M#]` da sostituire con la tua implementazione.

- **M1 — Simula la latenza di rete:**
  Implementa `NetworkLatencySimulator.WaitAsync` utilizzando `Task.Delay` con un
  intervallo casuale (es. 150-400 ms). Ricordati di propagare il `CancellationToken`.

- **M2 — Provider asincroni:**
  Completa i metodi `GetCurrentAsync`, `GetQuoteAsync` e `GetStatusAsync` nei rispettivi
  provider. Ogni metodo deve:
  1. Chiamare il `SimulatedApiClient` con `await`.
  2. Mappare i payload in `WeatherSummary`, `QuoteSummary` e `SystemStatusSummary`.
  3. Propagare il `CancellationToken`.

- **M3 — Aggregazione concorrente:**
  In `DashboardBuilder.BuildAsync` avvia le tre chiamate ai provider senza attenderle
  immediatamente, quindi usa `Task.WhenAll` (o equivalente) per ottenere tutti i dati.
  Con i risultati crea e restituisci un `DashboardSnapshot`.

- **M4 — Stream diagnostici:**
  Sempre in `DashboardBuilder`, implementa `StreamDiagnosticsAsync` per esporre
  gli eventi di `IDiagnosticsProvider` come `IAsyncEnumerable<DiagnosticEvent>`.
  Propaga la cancellazione e usa `await foreach` + `yield return`.

- **M5 — Programma principale asincrono:**
  In `Program.cs`:
  1. Attendi `BuildAsync` e stampa il risultato con `ToConsoleString()`.
  2. Itera `StreamDiagnosticsAsync` con `await foreach`, limitando l'output a 5 eventi.

---

## ▶️ Come eseguire

```bash
cd esercizi/AsyncAwaitDashboard/src/AsyncAwaitDashboard.App

# Facoltativo ma consigliato: ripristina dipendenze
# dotnet restore

dotnet run
```

Interrompi la diagnostica live con `Ctrl+C` oppure lasciando scadere il `CancellationToken`
creato nel `Program.cs`.

---

## 💡 Suggerimenti

* Mantieni i provider piccoli: ricevono payload grezzi, restituiscono record semplici.
* Non fare `Result`/`Wait()`: usa sempre `await`.
* Per la cancellazione annidata usa `using var cts = new CancellationTokenSource(...)`.
* Se vuoi testare con `dotnet test`, puoi creare tu stesso i test in una nuova cartella `tests/`.
* Sperimenta con `ConfigureAwait(false)` nei provider per vedere come cambia il contesto.

Buon divertimento! ⚙️
