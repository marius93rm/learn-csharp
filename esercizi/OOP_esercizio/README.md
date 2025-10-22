# Gestione Studenti — Esercizio guidato OOP in C#

Questo esercizio ti accompagna passo-passo nella creazione di un piccolo sistema per
la gestione degli studenti e dei loro voti. Ogni milestone introduce un concetto
chiave della programmazione orientata agli oggetti (OOP) in C# e ti invita a mettere
in pratica quanto appreso con codice e test automatici.

## Obiettivi didattici

- Comprendere classi, oggetti e incapsulamento.
- Gestire proprietà e metodi per modellare comportamenti.
- Applicare l'ereditarietà per estendere funzionalità.
- Utilizzare i test automatici con xUnit per verificare il codice.
- Introdurre la persistenza dei dati su file CSV.

## Struttura del repository

```
esercizi/OOP_esercizio/
├── Program.cs
├── README.md
├── Studente.cs
└── tests/
    └── StudenteTests.cs
```

Per semplicità il progetto utilizza un'unica libreria console con relativi test.
Puoi creare una soluzione locale con `dotnet new sln` e aggiungere i progetti se
preferisci, ma i file forniti sono già pronti per essere usati in un progetto
console standard più un progetto di test xUnit.

## Prerequisiti

1. **SDK .NET 7.0 o superiore** installato sulla tua macchina.
2. Familiarità di base con i concetti di `class`, `property`, `method` e con il
   pattern `List<T>` per memorizzare insiemi di dati.

---

## Milestone 1 — Classe base e proprietà

1. Apri `Studente.cs` e completa la classe `Studente`:
   - Aggiungi le proprietà auto-implementate `Nome` e `Cognome` (`string`).
   - Aggiungi la proprietà `Voti` come `List<int>`.
   - Implementa il costruttore che inizializza `Nome`, `Cognome` e la lista dei voti.
   - Implementa il metodo `AggiungiVoto(int voto)` per aggiungere un voto alla lista.
2. Usa i commenti `TODO` nel file per orientarti.

**Concetto chiave:** l'incapsulamento ti permette di controllare come vengono
modificati i dati interni di un oggetto.

---

## Milestone 2 — Calcolo della media

1. Implementa in `Studente` il metodo `CalcolaMedia()`.
2. Il metodo deve restituire la media aritmetica dei voti.
3. Gestisci il caso in cui non ci siano voti (ritorna `0` o stampa un messaggio).
4. In `Program.cs` stampa in console il nome completo dello studente e la media.

**Suggerimento:** sfrutta `Voti.Average()` o calcola manualmente somma e conteggio.
Ricorda di importare `System.Linq` se utilizzi LINQ.

---

## Milestone 3 — Test automatici (xUnit)

1. Nel progetto di test (`tests/StudenteTests.cs`) rimuovi l'attributo `Skip` dai
   test dopo aver completato le milestone precedenti.
2. Implementa i test per verificare che:
   - `AggiungiVoto` aggiunga effettivamente un voto.
   - `CalcolaMedia` restituisca la media corretta.
3. Esegui i test con `dotnet test`.

**Concetto chiave:** i test automatici sono una rete di sicurezza che ti aiuta a
confermare il corretto comportamento del codice mentre iteri sulle funzionalità.

---

## Milestone 4 — Ereditarietà (estensione opzionale)

1. Implementa la classe `StudenteUniversitario` che eredita da `Studente`.
2. Aggiungi la proprietà `CorsoDiLaurea`.
3. Sovrascrivi `CalcolaMedia()` per aggiungere un bonus di +1 se lo studente ha più
   di 5 voti.
4. Aggiorna `Program.cs` per creare almeno uno `StudenteUniversitario`.

**Concetto chiave:** l'ereditarietà ti permette di riutilizzare e specializzare il
comportamento di una classe di base.

---

## Milestone 5 — Persistenza su file (opzionale "wow")

1. Completa la classe `CsvRepository` per salvare i dati dello studente in un file
   `studenti.csv` con formato `Nome;Cognome;Media`.
2. Dopo aver calcolato la media in `Program.cs`, utilizza il repository per salvare
   ciascuno studente su file.

**Suggerimento:** usa `File.AppendAllLines` o `File.AppendAllText` per scrivere su
file e ricordati di gestire eventuali eccezioni I/O.

---

## Esecuzione

1. Compila: `dotnet build`
2. Esegui: `dotnet run`
3. Esegui i test: `dotnet test`

> I comandi assumono che tu abbia configurato due progetti: uno console e uno di
> test xUnit. Puoi crearli così:
>
> ```bash
> dotnet new console -n GestioneStudenti
> dotnet new xunit -n GestioneStudenti.Tests
> dotnet new sln -n GestioneStudenti
> dotnet sln GestioneStudenti.sln add GestioneStudenti/GestioneStudenti.csproj
> dotnet sln GestioneStudenti.sln add GestioneStudenti.Tests/GestioneStudenti.Tests.csproj
> dotnet add GestioneStudenti.Tests/GestioneStudenti.Tests.csproj reference GestioneStudenti/GestioneStudenti.csproj
> ```
>
> Quindi sposta i file di questo esercizio nelle rispettive cartelle e avrai un
> ambiente pronto per seguire le milestone.

---

## Output atteso

Ecco un esempio di output finale dopo aver completato tutte le milestone:

```
Studente: Luca Bianchi
Media voti: 27,5

Studente: Anna Rossi
Media voti: 30,0
Dati salvati su studenti.csv
```

Buon lavoro e buon divertimento con l'OOP in C#!
