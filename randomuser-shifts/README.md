# RandomUser Shifts — esercizio didattico TDD

Repository pensato per accompagnare passo dopo passo lo sviluppo di una mini applicazione console in **C# 12 / .NET 8** che usa l'API pubblica [`randomuser.me`](https://randomuser.me) per generare **turni di lavoro settimanali**. Il focus è sulla progettazione guidata dai principi **SOLID** (SRP, OCP, DIP, IoC/DI) e dall'uso di pattern classici (Adapter, Repository, Strategy, Observer), con test **TDD** che partono **rossi** grazie ai `TODO` sparsi nel codice.

```
┌────────────────┐      ┌────────────────────┐      ┌─────────────────────┐
│ Console (CLI)  │ ───▶ │  Application Layer │ ───▶ │ Strategies (OCP)    │
│ (Program.cs)   │      │  Scheduler service │      │ RoundRobin/Fairness │
└────────────────┘      └────────┬───────────┘      └──────────┬─────────┘
                                 │                             │
                                 ▼                             ▼
                         ┌───────────────┐             ┌────────────────┐
                         │ Repository    │◀──JSON──────│ File system    │
                         │ (Persistence) │             └────────────────┘
                                 ▲
                                 │ HTTP + DTO + Mapper
                                 ▼
                           RandomUser API
```

## Prerequisiti

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* Accesso a Internet per chiamare l'API `randomuser.me`

## Setup rapido

La soluzione è già configurata; basta ripristinare i pacchetti e compilare:

```bash
dotnet restore
dotnet build
```

## Esecuzione demo

```bash
dotnet run --project src/RandomUserShifts.App -- --people 6 --week 2025-01-13 --strategy round-robin
```

Output atteso dopo aver completato tutti i TODO:

```
RandomUser Shifts — Week 2025-01-13
Mon: 08:00-16:00 Ada Lovelace | 16:00-00:00 Grace Hopper
Tue: 08:00-16:00 Grace Hopper | 16:00-00:00 Ada Lovelace
...
Totali: Ada Lovelace=5 turni, Grace Hopper=4, ...
Salvato in ./data/schedules/2025-01-13.json
```

> ⚠️ Al momento l'app stampa solo un placeholder: i test `ConsoleOutputTests` guidano l'implementazione del riepilogo finale.

## Percorso didattico e Milestone

Ogni milestone introduce un concetto architetturale e sblocca uno o più test. L'ordine consigliato è **M1 → M7**, seguendo il ciclo **Red → Green → Refactor**. I numeri dei `TODO` indicano le azioni minime per far passare i test.

### M1 – Boot & SRP

* **Obiettivo**: la console parte, il parser argomenti (`ArgsParser`) è isolato, l'app stampa l'header.
* **Principio**: **Single Responsibility** – `ProgramEntrypoint` delega a `Bootstrap` (composition root) e a `ArgsParser`.
* **Test**: `ConsoleOutputTests` controlla la presenza dell'header "RandomUser Shifts".
* **TODO**: nessun TODO bloccante qui; verificare che la pipeline di avvio funzioni.

### M2 – Adapter API (DIP + Adapter)

* **Interfaccia**: `IRandomUserClient` restituisce `PersonDto`.
* **Implementazione**: `RandomUserHttpClient` usa `HttpClient` e modelli di risposta in `Http/Models`.
* **Mapper**: `PersonMapper` converte `PersonDto` in `Person`.
* **TODO[1]**: mappare correttamente `first + last`, `email`, `nat`, `login.uuid` → `Guid` stabile.
* **Test**: `RandomUserMappingTests` fallisce finché il mapper non è corretto.

### M3 – Regole & Strategy (OCP)

* **Concetto**: strategie intercambiabili per generare i turni.
* **File**: `ISchedulingStrategy`, `RoundRobinStrategy`, `FairnessStrategy`.
* **TODO[2]**: round-robin 7×2 turni (08:00–16:00, 16:00–00:00) rispettando `SchedulingRules`.
* **Test**: `StrategyRoundRobinTests` descrive i vincoli attesi (copertura giornaliera, rotazione, rispetto regole).

### M4 – Scheduler (Application Service)

* **Classe**: `Scheduler` coordina strategia, regole e persone.
* **TODO[4]**: validare input (almeno una persona), evitare che la strategia parta con dati inconsistenti, allegare `rules.Describe()` al `Schedule`.
* **Test**: `SchedulerTests` vuole un'eccezione con zero persone e la presenza delle regole applicate.

### M5 – Repository (DIP + SRP)

* **Interfaccia**: `IScheduleRepository` con `SaveAsync` / `LoadAsync`.
* **Implementazione**: `FileScheduleRepository` salva JSON in `./data/schedules/{yyyy-MM-dd}.json`.
* **TODO[5]**: serializzazione, path naming, creazione cartelle mancanti.
* **Test**: `RepositoryJsonTests` usa una fixture temporanea per verificare salvataggio/caricamento.

### M6 – Composition Root & CLI (IoC/DI)

* **Componenti**: `Bootstrap` costruisce `HttpClient`, repository, strategie.
* **CLI**: `ArgsParser` gestisce `--people`, `--week`, `--strategy`.
* **TODO[6]**: weekStart di default = prossimo lunedì se l'utente non specifica `--week`.
* **TODO[7]**: stampare riepilogo giornaliero e totali per persona usando `IConsoleWriter` (testabile).
* **Test**: `ConsoleOutputTests` attende 7 righe giorno + tabella totali.

### M7 – Refactor & Estensioni (OCP)

* **Strategia Fairness**: nuova implementazione che minimizza la varianza dei turni.
* **TODO[3]**: completare l'algoritmo abbozzato.
* **Test**: `StrategyFairnessTests` verifica differenza ≤ 1 turno tra le persone.
* **Estendibilità**: aggiungere nuove strategie senza toccare l'applicazione.

## Workflow TDD suggerito

1. **Red**: eseguire `dotnet test` e osservare i fallimenti.
2. **Green**: completare il `TODO` numerato, rieseguire i test.
3. **Refactor**: migliorare il codice mantenendo i test verdi.
4. Ripetere per il `TODO` successivo.

```bash
dotnet test
```

### Ordine suggerito dei TODO

1. `TODO[1]` – mapping RandomUser → Person.
2. `TODO[2]` – RoundRobin funzionante.
3. `TODO[4]` – validazioni Scheduler.
4. `TODO[5]` – persistenza JSON.
5. `TODO[6]` – default CLI next Monday.
6. `TODO[7]` – riepilogo console.
7. `TODO[3]` – strategia Fairness bilanciata (milestone finale).

## Troubleshooting

* **Certificati HTTPS**: se `HttpClient` fallisce, configurare `DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0` o aggiungere certificati di sistema.
* **Percorsi**: su Windows, verificare i permessi di scrittura in `./data/schedules`. Il repository crea la cartella se manca (TODO[5]).
* **Fusi orari**: gli orari sono in `TimeOnly`, indipendenti dal fuso locale. Evitare `DateTime.Now` nei test.
* **API rate limiting**: `randomuser.me` è pubblico ma può limitare richieste. In test usare i fake client (vedi `ConsoleOutputTests`).

## Idee di estensione

* Aggiungere regole extra (riposo weekend, max ore settimanali).
* Esportare in CSV o inviare notifiche email.
* Introdurre una progress bar in console.
* Integrare un container di DI (Microsoft.Extensions.DependencyInjection).
* Implementare un evento `ScheduleSaved` nel repository per notificare altre componenti.

Buon divertimento con TDD e architettura pulita! 🎯
