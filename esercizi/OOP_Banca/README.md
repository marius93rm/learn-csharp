# Gestione Banca — Esercizio OOP (C#)

## Teoria breve
L'obiettivo dell'esercizio è mettere in pratica concetti chiave della programmazione orientata agli oggetti in C#:

- **Classi e oggetti**: modellare entità del dominio "banca" (Clienti, Conti, Transazioni) come tipi concreti.
- **Incapsulamento**: mantenere i dati consistenti attraverso proprietà e metodi pubblici controllati.
- **Ereditarietà**: riutilizzare il comportamento comune in una classe base astratta (`ContoBancario`) e specializzare nelle derivate.
- **Polimorfismo**: invocare metodi (`Preleva`, `StampaEstrattoConto`) su riferimenti alla classe base e ottenere comportamenti specifici dell'istanza concreta.
- **Classi astratte vs derivate**: una classe astratta definisce il contratto comune (proprietà, metodi astratti/virtuali) che le classi figlie devono implementare o estendere.

## Consegna
Implementa i `// TODO [M#]: ...` presenti nel codice sorgente per completare le milestone e far funzionare gli esempi in `Program.cs` (e i test nella cartella `tests` se decidi di eseguirli).

## Milestones
- **M1**: Crea `Cliente` con proprietà in sola lettura e costruttore che valida i parametri.
- **M2**: Crea `ContoBancario` (abstract) con: `NumeroConto`, `Titolare`, `Saldo` (setter protetto), `List<Transazione>`; metodi `Deposita(decimal)`, `Preleva(decimal)` (virtuale o astratto), `StampaEstrattoConto()`.
- **M3**: Crea `ContoCorrente` (derivata) con opzionale `decimal Fido`; override di `Preleva` consentendo saldo negativo entro il fido.
- **M4**: Crea `ContoRisparmio` (derivata) con `decimal TassoInteresseAnnuale`; metodo `ApplicaInteressi()` che aumenta il saldo registrando la transazione degli interessi.
- **M5**: Aggiungi `Transazione` (Data, Importo, Tipo: `"DEPOSITO"`/`"PRELIEVO"`/`"INTERESSI"`, Descrizione) e registra ogni operazione.
- **M6**: In `Program.cs` istanzia un cliente, crea un conto corrente e un conto risparmio, esegui una sequenza di operazioni e stampa l'estratto conto di ciascun conto in modo polimorfico.

## Esempio teorico (non la soluzione)
```csharp
// Esempio teorico (non la soluzione):
abstract class ContoBancario {
    public string NumeroConto { get; }
    public Cliente Titolare { get; }
    public decimal Saldo { get; protected set; }
    public abstract void Preleva(decimal importo);
    public void Deposita(decimal importo) { /* ...validazioni... */ }
}
class ContoCorrente : ContoBancario {
    public decimal Fido { get; }
    public override void Preleva(decimal importo) { /* ... */ }
}
```

## Istruzioni di esecuzione
1. `dotnet build`
2. `dotnet run --project src/BankApp/BankApp.csproj`
3. Facoltativo: `dotnet test`

## Suggerimenti
- Utilizza il tipo `decimal` per gli importi monetari.
- Fornisci messaggi d'errore esplicativi nelle eccezioni.
- Non introdurre logica di I/O su file o database: mantieni tutto in memoria per semplicità.
