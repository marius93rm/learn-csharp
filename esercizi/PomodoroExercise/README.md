# 🕒 Pomodoro Focus Timer — Esercizio con SOLID + Test

Benvenuto! In questo esercizio costruirai un **Pomodoro Focus Timer** in C# applicando i principi **SOLID** in modo concreto.
Il progetto include **test automatici** per verificare il tuo lavoro e un file con porzioni **TODO** da completare.

---

## 🎯 Obiettivi didattici

* Applicare **SRP, OCP, DIP, ISP** su un caso reale.
* Usare **interfacce** e piccole astrazioni per separare UI, logica e persistenza.
* Scrivere codice **testabile** con dipendenze iniettabili.
* Eseguire **test xUnit** per validare il comportamento.

---

## 📦 Struttura progetto

```
PomodoroExercise/
├─ src/
│  └─ Pomodoro.App/
│     ├─ Program.cs
│     ├─ Timer/
│     │  ├─ ITickProvider.cs
│     │  ├─ RealTickProvider.cs
│     │  └─ TimerService.cs        <-- TODO principali
│     ├─ Notify/
│     │  ├─ INotifier.cs
│     │  ├─ ConsoleNotifier.cs
│     │  └─ BeepNotifier.cs
│     ├─ Sessions/
│     │  ├─ ISessionStrategy.cs
│     │  ├─ Classic25_5.cs
│     │  ├─ Deep50_10.cs
│     │  └─ Custom.cs
│     ├─ Persistence/
│     │  ├─ ISessionRepository.cs
│     │  └─ CsvSessionRepository.cs
│     ├─ Core/
│     │  ├─ Pomodoro.cs            <-- TODO
│     │  └─ PomodoroRunner.cs
│     └─ Pomodoro.App.csproj
└─ tests/
   └─ Pomodoro.Tests/
      ├─ PomodoroTests.cs
      └─ Pomodoro.Tests.csproj
```

---

## 🔎 Breve teoria (con esempio)

* **SRP**: una classe = una responsabilità (es. `TimerService` conta il tempo, **non** invia notifiche).
* **DIP**: dipendi da **interfacce**, non da classi concrete (es. `Pomodoro` dipende da `INotifier`, non da `Console.WriteLine`).
* **OCP**: estendi con nuove strategie senza toccare il codice esistente (es. `ISessionStrategy` → `Classic25_5`, `Deep50_10`, `Custom`).
* **ISP**: interfacce **piccole e mirate** (es. `INotifier` con un solo metodo).

**Esempio minimo di DIP + OCP:**

```csharp
public interface INotifier { void Notify(string message); }

public class ConsoleNotifier : INotifier {
    public void Notify(string message) => Console.WriteLine(message);
}

public interface ISessionStrategy { (int focusSec, int breakSec) GetDurations(); }
public class Classic25_5 : ISessionStrategy { public (int,int) GetDurations() => (25*60, 5*60); }
```

---

## 🧱 Milestones

1. **Timer minimo (SRP)** — Implementa `TimerService.Countdown` usando `ITickProvider` per scandire i secondi.
2. **Notifiche (DIP + ISP)** — Usa `INotifier` per inviare messaggi di fine sessione.
3. **Strategie (OCP)** — Implementa `ISessionStrategy` per durate `25/5` e `50/10` (+ custom).
4. **Evento completamento (Observer)** — In `Pomodoro`, pubblica un evento quando il focus finisce.
5. **Persistenza (SRP + DIP)** — `CsvSessionRepository` salva il log delle sessioni in `sessions.csv`.
6. **Runner** — In `PomodoroRunner`, esegui focus e break in sequenza mostrando il countdown.
7. **(Extra “wow”) Progress bar** — opzionale: aggiungi una barra di avanzamento in console.

---

## ▶️ Come eseguire

Assicurati di avere **.NET 8 SDK** installato.

```bash
dotnet build
dotnet test
dotnet run --project src/Pomodoro.App
```

---

## ✅ Cosa verificano i test

* Che `TimerService` chiami `onTick` correttamente e `onCompleted` alla fine.
* Che `Pomodoro` notifichi al termine del focus.
* Che le **strategie** restituiscano le durate attese.
* Che il **repository CSV** salvi una riga ben formata.

> Nota: i test sono progettati perché inizialmente **falliscano** finché non completi i TODO.