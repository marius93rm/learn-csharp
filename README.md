# Learn C# — repository corso

Materiale completo per studiare e insegnare **C#**: slide, compendi, esercizi guidati con milestone e (dove previsti) test automatici.

## Sommario

- [Contenuto del repository](#contenuto-del-repository)
- [Requisiti](#requisiti)
- [Come avviare un esercizio](#come-avviare-un-esercizio)
- [Struttura consigliata per ogni esercizio](#struttura-consigliata-per-ogni-esercizio)
- [Roadmap consigliata](#roadmap-consigliata)
- [Linee guida didattiche](#linee-guida-didattiche)
- [Esercizi inclusi](#esercizi-inclusi)
- [Suggerimenti per macOS (VS Code)](#suggerimenti-per-macos-vs-code)
- [Quality checklist per docenti e mentor](#quality-checklist-per-docenti-e-mentor)
- [Contribuire--ampliare](#contribuire--ampliare)

## Contenuto del repository

```text
learn-csharp/
├── check.pdf               # scheda riassuntiva per docenti/mentor
├── esempi/                 # snippet e mini progetti dimostrativi
│   └── asincrona/          # esempi pratici su async/await
├── esercizi/               # esercizi guidati con README e milestone
│   ├── AsyncAwaitDashboard/
│   ├── GestoreMagazzino.TDD/
│   ├── LogEventiGuidato/
│   ├── OOP_Banca/
│   ├── OOP_esercizio/
│   ├── PomodoroExercise/
│   ├── ValidazioneModelliGuidato/
│   ├── design-patterns-todo/
│   │   ├── Patterns/       # file con i TODO sui pattern GoF + soluzioni dedicate
│   │   └── Solutions/
│   ├── design-patterns-todo2/
│   │   ├── Patterns/
│   │   └── Solutions/
│   ├── microservices/
│   │   ├── Gateway/        # API gateway che orchestra i servizi
│   │   ├── OrderService/
│   │   ├── Shared/         # DTO e interfacce comuni
│   │   └── UserService/
│   ├── randomuser-shifts/  # progetto TDD completo con test automatici
│   └── randomuser-solution/  # soluzione commentata per lo stesso dominio RandomUser
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

1. Clona o scarica il repository: `git clone https://github.com/<org>/learn-csharp.git`.
2. Apri la cartella dell’esercizio su cui vuoi lavorare, ad esempio:

   ```bash
   cd esercizi/PomodoroExercise/src/Pomodoro.App
   ```

3. Ripristina le dipendenze e compila per verificare che il progetto sia sano:

   ```bash
   dotnet restore
   dotnet build
   ```

4. Avvia l’applicazione o il progetto console:

   ```bash
   dotnet run
   ```

5. Se l’esercizio ha test automatici, torna alla radice dell’esercizio ed eseguili:

   ```bash
   cd ../../..   # torna alla radice dell'esercizio
   dotnet test
   ```

6. Alcuni esercizi includono script `watch` o README con comandi extra: leggili sempre prima di iniziare.

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

## Roadmap consigliata

Per ottenere una curva di apprendimento graduale, proponiamo la seguente sequenza (adatta sia a studenti autonomi sia a percorsi guidati):

1. **Propedeutica** — studia le slide introduttive (`slide/C# Propedeutico 1.pdf`) e risolvi `esercizi/OOP_esercizio/`.
2. **OOP intermedio** — affronta `esercizi/OOP_Banca/` per consolidare classi, ereditarietà e test.
3. **Async & TPL** — passa a `esercizi/AsyncAwaitDashboard/` e rivedi gli esempi in `esempi/asincrona/`.
4. **SOLID e design pattern** — completa progressivamente `design-patterns-todo/` e `design-patterns-todo2/` supportandoti con le slide dedicate.
5. **TDD & architetture** — esercitati con `GestoreMagazzino.TDD/` e concludi con il percorso `microservices/`.

> Ogni tappa può essere proposta come workshop di mezza giornata: includi sempre un recap teorico + live coding + pairing.

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
| Log eventi guidato | `esercizi/LogEventiGuidato/` | Verifica manuale step-by-step eseguendo i progetti `src/Step1`…`Step4` | Attributi personalizzati, reflection, generics con vincoli | `slide/Attributi.pdf`, `slide/Reflection, attributi e generics avanzati.pdf` |
| Validazione modelli guidato | `esercizi/ValidazioneModelliGuidato/` | Verifica manuale completando i TODO negli step `Step1`…`Step4` | Attributi, reflection, interfacce, validatori generici, SOLID | `slide/Attributi.pdf`, `slide/Reflection, attributi e generics avanzati.pdf` |
| Design Patterns TODO (parte 1) | `esercizi/design-patterns-todo/` | Verifica manuale eseguendo `dotnet run` e completando i `// TODO` nei file `Patterns/*.cs` | Pattern GoF fondamentali, refactoring, astrazioni | `slide/Design Patterns.pdf` |
| Design Patterns TODO (parte 2) | `esercizi/design-patterns-todo2/` | Verifica manuale eseguendo `dotnet run` e completando i `// TODO` nei file `Patterns/*.cs` | Pattern GoF avanzati, thread-safety, architetture modulari | `slide/Design Patterns part 2.pdf` |
| Microservices Playground | `esercizi/microservices/` | Esercizio guidato senza test automatici: seguire i TODO nelle cartelle `Gateway/`, `UserService/`, `OrderService/` | Microservizi, API gateway, DTO condivisi, resilienza, DIP | `slide/Design Pattern, SOLID, IoC, DI e Microservizi.pdf` |
| RandomUser Shifts (TDD) | `esercizi/randomuser-shifts/` | Portare a verde i test in `tests/RandomUserShifts.Tests/` seguendo l'ordine delle milestone `TODO[#]` | SOLID, pattern (Adapter, Strategy, Repository), HttpClient, TDD avanzato | `slide/Design Pattern, SOLID, IoC, DI e Microservizi.pdf`, `slide/TDD.pdf` |

### Risorse aggiuntive

* `esercizi/randomuser-solution/` — soluzione console completa e commentata per l'esercizio RandomUser, utile come materiale di studio o confronto.

## Suggerimenti per macOS (VS Code)

* Installazione rapida:

  ```bash
  brew install --cask dotnet-sdk
  dotnet --info
  ```
* Se VS Code non vede il SDK, riavvia l’editor e/o esegui `dotnet new console -n Probe && rm -rf Probe`.

## Quality checklist per docenti e mentor

* [ ] README dell’esercizio con obiettivi, milestone e criteri.
* [ ] Compilazione pulita (`dotnet build`).
* [ ] Test esistenti verdi (`dotnet test`).
* [ ] Nessuna logica hard-coded in Program.cs.
* [ ] Nomi espliciti, metodi brevi, nessuna classe “God”.

## Contribuire / Ampliare

1. Apri una issue o descrivi il contesto nel canale di riferimento (Slack/Teams) per raccogliere feedback preliminare.
2. Crea un branch dedicato, mantieni commit piccoli e descrittivi.
3. Se modifichi esercizi esistenti, aggiorna sempre README e test per evitare regressioni.
4. Segui la checklist qui sopra prima di aprire una Pull Request.
5. Le PR dovrebbero includere screenshot o GIF per componenti UI e riferimenti alle slide aggiornate, se applicabile.

* Duplica uno scheletro esercizio e adatta obiettivi/milestone.
* Mantieni la stessa **struttura** e lo **stile dei README**.
* Se aggiungi slide o compendi, inseriscili in `slide/` con naming coerente.

© 2025 Marius Minia
