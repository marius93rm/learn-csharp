# Learn C# — repository corso

Materiale completo per studiare e insegnare **C#**: slide, compendi, esercizi guidati con milestone e (dove previsti) test automatici.

## Contenuto del repository

```text
learn-csharp/
├── check.pdf               # scheda riassuntiva per docenti/mentor
├── esempi/                 # snippet e mini progetti dimostrativi
│   └── asincrona/          # esempi pratici su async/await
├── esercizi/               # esercizi guidati con README e milestone
│   ├── AsyncAwaitDashboard/
│   ├── GestoreMagazzino.TDD/
│   ├── OOP_Banca/
│   ├── OOP_esercizio/
│   ├── PomodoroExercise/
│   ├── design-patterns-todo/
│   │   ├── Patterns/       # file con i TODO sui pattern GoF + soluzioni dedicate
│   │   └── Solutions/
│   ├── design-patterns-todo2/
│   │   ├── Patterns/
│   │   └── Solutions/
│   └── microservices/
│       ├── Gateway/        # API gateway che orchestra i servizi
│       ├── OrderService/
│       ├── Shared/         # DTO e interfacce comuni
│       └── UserService/
├── slide/                  # materiale teorico in PDF (propedeutici, OOP, SOLID, pattern, microservizi…)
└── README.md               # questo file introduttivo
```

> Convenzione nomi: ogni esercizio include un **README dedicato**, sorgenti nella sottocartella `src/`, eventuali `tests/` e spesso una cartella `soluzione/` con una possibile implementazione completa.

## Requisiti

* .NET SDK (consigliato .NET 8):
  macOS / Windows / Linux → scarica da dotnet.microsoft.com
* Editor: **VS Code** (consigliato) o Visual Studio.
* Estensioni utili: C# Dev Kit, .NET Test Explorer.

## Come avviare un esercizio

1. Apri la cartella dell’esercizio, es:
   `cd esercizi/PomodoroExercise/src/Pomodoro.App`
2. Esegui:

   ```bash
   dotnet run
   ```
3. Se l’esercizio ha test:

   ```bash
   cd ../../..   # torna alla radice dell'esercizio
   dotnet test
   ```

## Struttura consigliata per ogni esercizio

```
<ExerciseName>/
├── README.md                 # istruzioni, milestone, criteri di valutazione
├── src/<ProjectName>/
│   ├── *.cs                  # codice da completare
│   └── <ProjectName>.csproj
├── tests/                    # xUnit o simili (opzionale ma consigliato)
└── <ExerciseName>.sln
```

### Linee guida didattiche

* **Milestone piccole** e verificabili (core → persistenza → interfacce/DI → notifiche/UI → test).
* **Niente logica in Program.cs**: usa classi/servizi, facilita i test.
* **DIP first**: dipendi da **interfacce**, inietta implementazioni concrete.
* **SRP/OCP**: classi piccole, estendi senza modificare.

## Esercizi inclusi

| Esercizio | Percorso | Test da implementare | Argomenti trattati | Slide di riferimento |
|-----------|----------|----------------------|--------------------|----------------------|
| Pomodoro Focus Timer | `esercizi/PomodoroExercise/` | Completare i TODO in `tests/Pomodoro.Tests/` e mantenere verdi i test xUnit forniti | SOLID, DIP/IoC, repository su file, timer e notifiche | `slide/Design Pattern, SOLID, IoC, DI e Microservizi.pdf` |
| Async Await Dashboard | `esercizi/AsyncAwaitDashboard/` | Test non forniti di default (facoltativo crearli) | `async/await`, `Task.WhenAll`, `CancellationToken`, `IAsyncEnumerable` | `slide/Async&Await.pdf` |
| GestoreMagazzino.TDD | `esercizi/GestoreMagazzino.TDD/` | Scrivere i test indicati dai TODO `*.T#` in `tests/InventarioTests.cs` seguendo il ciclo TDD | TDD, SRP, DIP, refactoring, Moq | `slide/TDD.pdf` |
| Gestione Banca (OOP) | `esercizi/OOP_Banca/` | Aggiornare i test in `tests/Bank.Tests/` sostituendo i TODO [Test] con asserzioni reali | OOP, incapsulamento, ereditarietà, polimorfismo, gestione transazioni | `slide/C# - 2.pdf` |
| Gestione Studenti (OOP) | `esercizi/OOP_esercizio/` | Rimuovere gli `Skip` e completare i test in `tests/StudenteTests.cs` | OOP di base, liste, ereditarietà opzionale, persistenza CSV | `slide/C# Propedeutico 1.pdf` |
| Design Patterns TODO (parte 1) | `esercizi/design-patterns-todo/` | Verifica manuale eseguendo `dotnet run` e completando i `// TODO` nei file `Patterns/*.cs` | Pattern GoF fondamentali, refactoring, astrazioni | `slide/Design Patterns.pdf` |
| Design Patterns TODO (parte 2) | `esercizi/design-patterns-todo2/` | Verifica manuale eseguendo `dotnet run` e completando i `// TODO` nei file `Patterns/*.cs` | Pattern GoF avanzati, thread-safety, architetture modulari | `slide/Design Patterns part 2.pdf` |
| Microservices Playground | `esercizi/microservices/` | Esercizio guidato senza test automatici: seguire i TODO nelle cartelle `Gateway/`, `UserService/`, `OrderService/` | Microservizi, API gateway, DTO condivisi, resilienza, DIP | `slide/Design Pattern, SOLID, IoC, DI e Microservizi.pdf` |

## Suggerimenti per macOS (VS Code)

* Installazione rapida:

  ```bash
  brew install --cask dotnet-sdk
  dotnet --info
  ```
* Se VS Code non vede il SDK, riavvia l’editor e/o esegui `dotnet new console -n Probe && rm -rf Probe`.

## Quality checklist (per PR/esercizi)

* [ ] README dell’esercizio con obiettivi, milestone e criteri.
* [ ] Compilazione pulita (`dotnet build`).
* [ ] Test esistenti verdi (`dotnet test`).
* [ ] Nessuna logica hard-coded in Program.cs.
* [ ] Nomi espliciti, metodi brevi, nessuna classe “God”.

## Contribuire / Ampliare

* Duplica uno scheletro esercizio e adatta obiettivi/milestone.
* Mantieni la stessa **struttura** e lo **stile dei README**.
* Se aggiungi slide o compendi, inseriscili in `slide/` con naming coerente.

© 2025 Marius Minia
