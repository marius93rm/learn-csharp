# Rate Limiter asincrono – Soluzione di riferimento

Questa cartella contiene una possibile soluzione completa per il progetto didattico
"Rate Limiter asincrono – SemaphoreSlim e Task". Il codice qui presente implementa
tutte le parti lasciate come TODO nella versione esercitativa, fornendo un
esempio di come orchestrare `SemaphoreSlim`, la finestra temporale e la gestione
dei `CancellationToken`.

La struttura replica quella del progetto originale:

```
RateLimiterSolution/
├── RateLimiter.sln
└── src/
    └── RateLimiter.App/
        ├── RateLimiter.App.csproj
        ├── Program.cs
        ├── Models/
        ├── Services/
        └── Utilities/
```

Per compilare ed eseguire la soluzione:

```bash
cd RateLimiterSolution/src/RateLimiter.App
dotnet run
```

La logica del rate limiter accetta le richieste fino al raggiungimento dei limiti
configurati e restituisce un riepilogo dettagliato dei risultati.
