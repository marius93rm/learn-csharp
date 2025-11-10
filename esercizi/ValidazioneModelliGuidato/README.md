# Percorso guidato: Validazione automatica dei modelli

Benvenuto nel percorso guidato dedicato alla costruzione di un **sistema di validazione automatica dei modelli** in C#. L'esercizio è pensato per studenti con livello intermedio-avanzato e segue la struttura progressiva del corso: ogni step introduce nuove tecniche e concetti, lasciando sezioni TODO da completare.

## Obiettivi didattici
- Creare attributi personalizzati per dichiarare regole di validazione.
- Applicare reflection per leggere in modo dinamico gli attributi dalle proprietà del modello.
- Definire l'interfaccia `IValidabile` e implementare il metodo `Valida(out List<string> errori)`.
- Scrivere un validatore generico `Validatore<T>` con vincolo `where T : IValidabile`.
- Rispettare i principi SOLID: separare il modello dal validatore (SRP) e rendere il sistema estendibile a nuovi attributi senza modificare il validatore (OCP).

## Struttura del percorso
Il percorso è composto da quattro progetti console indipendenti, uno per ciascun step. Ogni progetto contiene:
- una breve introduzione didattica all'inizio del file `Program.cs`;
- codice parzialmente completo con commenti ed esercizi TODO;
- un metodo `Main()` per eseguire subito lo step;
- esempi di output (se rilevanti) per verificare il proprio lavoro.

| Step | Contenuto | Focus principale |
| ---- | --------- | ---------------- |
| 1 | `Step1/Program.cs` | Creazione dell'attributo `[Obbligatorio]` e suo utilizzo nel modello `Utente`. |
| 2 | `Step2/Program.cs` | Introduzione dell'interfaccia `IValidabile` e implementazione di `Valida()` leggendo `[Obbligatorio]` con reflection. |
| 3 | `Step3/Program.cs` | Implementazione della classe generica `Validatore<T>` che esegue `Valida()` sulle istanze. |
| 4 | `Step4/Program.cs` | Estensione con l'attributo `[MinLunghezza(int)]` e aggiornamento di `Valida()` seguendo il principio OCP. |

## Come utilizzare gli step
1. Apri ciascuna cartella (`Step1`, `Step2`, `Step3`, `Step4`).
2. Completa le sezioni contrassegnate da `// TODO` seguendo le spiegazioni presenti nei commenti.
3. Esegui il progetto corrispondente per verificare il risultato.

Esempio di esecuzione (assicurati di avere .NET 6 o versione successiva installata):

```bash
dotnet run --project esercizi/ValidazioneModelliGuidato/Step1/ValidazioneStep1.csproj
```

Ripeti il comando per ogni step sostituendo il percorso del progetto.

## Suggerimenti per l'apprendimento
- Lavora in ordine: ogni step si basa sui concetti introdotti nel precedente.
- Rileggi i commenti di spiegazione quando incontri un TODO: ti guideranno nella soluzione.
- Sperimenta aggiungendo proprietà o attributi personalizzati per rafforzare la comprensione del principio OCP.

Buon lavoro e buona sperimentazione!
