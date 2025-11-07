# Esercizio guidato: mini-sistema di log eventi

Questo percorso in quattro step accompagna lo studente nella creazione di un piccolo sistema di log basato su attributi personalizzati, reflection e generics con vincoli. Ogni step è un progetto console .NET 6 indipendente con codice da completare (commenti `TODO`).

Per eseguire uno step:

```bash
cd esercizi/LogEventiGuidato/src/StepX
dotnet run
```

Sostituisci `StepX` con lo step che vuoi avviare.

---

## Step 1 – Attributo `[Loggable]` e decorazione dei metodi
**Obiettivo didattico:** creare un attributo personalizzato da applicare ai metodi che desideriamo monitorare.

File coinvolti:
- [`LoggableAttribute.cs`](src/Step1/LoggableAttribute.cs)
- [`ServizioOrdini.cs`](src/Step1/ServizioOrdini.cs)
- [`Program.cs`](src/Step1/Program.cs)

Attività principali (vedi i `TODO` nei file):
1. Aggiungere eventuali proprietà opzionali all'attributo.
2. Applicare `[Loggable]` ai metodi da monitorare.
3. Integrare un secondo metodo loggabile con parametri.
4. Richiamare i metodi loggabili da `Program` per verificare manualmente l'attributo.

---

## Step 2 – Reflection per individuare i metodi `[Loggable]`
**Obiettivo didattico:** usare la reflection per trovare e invocare automaticamente tutti i metodi decorati.

File coinvolti:
- [`LoggableAttribute.cs`](src/Step2/LoggableAttribute.cs)
- [`ServizioOrdini.cs`](src/Step2/ServizioOrdini.cs)
- [`ReflectionRunner.cs`](src/Step2/ReflectionRunner.cs)
- [`Program.cs`](src/Step2/Program.cs)

Attività principali:
1. Popolare l'elenco dei metodi che hanno l'attributo `LoggableAttribute`.
2. Stampare la descrizione recuperata dall'attributo.
3. Invocare dinamicamente ciascun metodo individuato.
4. Aggiungere un metodo non loggabile per confrontarne il comportamento.

---

## Step 3 – Interfaccia `IEventoLoggabile` e classe evento
**Obiettivo didattico:** definire un contratto comune per gli eventi di log e implementarne una prima versione concreta.

File coinvolti:
- [`IEventoLoggabile.cs`](src/Step3/IEventoLoggabile.cs)
- [`EventoAccesso.cs`](src/Step3/EventoAccesso.cs)
- [`Program.cs`](src/Step3/Program.cs)

Attività principali:
1. Completare l'interfaccia con proprietà/metodi significativi.
2. Implementare `EventoAccesso` valorizzando timestamp, messaggi e un identificatore di tipo.
3. Sostituire i placeholder (`TODO`) con logica applicativa reale.
4. Istanziate un evento in `Program` e stampatelo in console.

---

## Step 4 – Classe generica `Logger<T>` con vincoli
**Obiettivo didattico:** creare un logger tipizzato che gestisca qualsiasi evento che implementi `IEventoLoggabile`, usando vincoli `where` (incluso `new()`).

File coinvolti:
- [`IEventoLoggabile.cs`](src/Step4/IEventoLoggabile.cs)
- [`EventoAccesso.cs`](src/Step4/EventoAccesso.cs)
- [`Logger.cs`](src/Step4/Logger.cs)
- [`Program.cs`](src/Step4/Program.cs)

Attività principali:
1. Assicurare che `Logger<T>` crei nuove istanze grazie al vincolo `new()`.
2. Applicare le configurazioni passate dalla lambda a ciascun evento registrato.
3. Salvare gli eventi in una lista interna e restituirli filtrati su richiesta.
4. Popolare il logger in `Program` e mostrare l'uso del filtro opzionale.

> Suggerimento: una volta completati tutti gli step, prova a collegare la reflection dello Step 2 con il logger generico dello Step 4 per ottenere un sistema di log completo.
