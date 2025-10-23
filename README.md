# Learn C# — repository corso

Materiale completo per studiare e insegnare **C#**: slide, compendi, esercizi guidati con milestone e (dove previsti) test automatici.

## Contenuto

* `slide/` — PDF propedeutici e avanzati (C# 1–3, Propedeutico 1–3, Design Pattern/SOLID/IoC/DI, Indice argomenti avanzati).
* `esercizi/` — raccolta di esercizi a difficoltà crescente.

  * `PomodoroExercise/` — esercizio su SOLID, repository/persistenza e test.
  * `AsyncAwaitDashboard/` — dashboard asincrona con `Task`, `async/await`, cancellazione e stream.
  * *(altri esercizi verranno aggiunti qui con la stessa struttura)*
* `Compendio Pomodoro C#.pdf` — compendio teorico+pratico d’esempio.
* `README.md` — questo file.

> Convenzione nomi: ogni esercizio ha una cartella con **README dedicato**, soluzione guidata e, se previsto, **tests**.

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

## Esercizi inclusi (work-in-progress)

* **PomodoroExercise**
  Focus su: SRP/OCP/ISP/DIP, repository CSV, notifiche, xUnit.
  Path: `esercizi/PomodoroExercise/` (leggi il README dentro la cartella).

* **AsyncAwaitDashboard**
  Focus su: `async`/`await`, `Task.WhenAll`, `CancellationToken`, `IAsyncEnumerable`.
  Path: `esercizi/AsyncAwaitDashboard/`.

*(Aggiungi qui un bullet per ogni nuovo esercizio con tema e path.)*

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