# Soluzione guidata con commenti

Questa cartella contiene una possibile implementazione completa dello
scenario "LogEventiGuidato". Ogni step riprende gli stessi progetti presenti
nella cartella `src/` ma con il codice già completato e ampiamente
commentato. L'obiettivo è fornire un riferimento da consultare dopo aver
provato a risolvere i TODO in autonomia.

Di seguito una panoramica dettagliata dei passaggi affrontati.

## Step 1 – Attributo personalizzato e decorazione manuale

**File principali:**
- `Step1/LoggableAttribute.cs`
- `Step1/ServizioOrdini.cs`
- `Step1/Program.cs`

Punti chiave:
1. L'attributo `[Loggable]` è stato reso **configurabile** tramite la proprietà
   opzionale `Descrizione`. Questo permette di arricchire il log con un testo
   esplicativo senza obbligare ogni metodo a fornirlo.
2. In `ServizioOrdini` abbiamo decorato due metodi con l'attributo. Il secondo
   accetta un parametro così da mostrare che l'attributo non altera la firma del
   metodo.
3. `Program` richiama manualmente i metodi loggati per evidenziare la differenza
   rispetto ai metodi non decorati. In questo step non c'è ancora automazione,
   l'obiettivo è solamente comprendere come applicare e usare un attributo
   personalizzato.

## Step 2 – Individuazione automatica via reflection

**File principali:**
- `Step2/LoggableAttribute.cs`
- `Step2/ServizioOrdini.cs`
- `Step2/ReflectionRunner.cs`
- `Step2/Program.cs`

Punti chiave:
1. `ReflectionRunner` usa `GetMethods` con i corretti `BindingFlags` per
   raccogliere tutti i metodi marcati con `[Loggable]`.
2. Per ogni metodo trovato leggiamo la proprietà `Descrizione` dell'attributo e
   stampiamo un messaggio introduttivo prima di eseguire il metodo stesso.
3. Nell'esempio gli handler non hanno parametri: in caso contrario sarebbe
   sufficiente ampliare il blocco che prepara l'array `argomenti` (il commento
   nel codice suggerisce proprio questa estensione).
4. `ServizioOrdini` contiene volutamente un metodo non decorato (`PuliziaCache`)
   per dimostrare che la reflection lo ignora.

## Step 3 – Contratto degli eventi e prima implementazione concreta

**File principali:**
- `Step3/IEventoLoggabile.cs`
- `Step3/EventoAccesso.cs`
- `Step3/Program.cs`

Punti chiave:
1. L'interfaccia `IEventoLoggabile` definisce il **contratto minimo** per qualsiasi
   evento: `Timestamp`, `Messaggio` e `Categoria`.
2. `EventoAccesso` implementa l'interfaccia e aggiunge campi utili per il dominio
   (nome utente e risultato dell'autenticazione). Il costruttore valorizza subito
   il timestamp con `DateTime.UtcNow`.
3. L'override di `ToString` produce una stringa pronta per il log, con tutte le
   informazioni principali.
4. In `Program` creiamo due eventi (uno riuscito e uno fallito) per mostrare i
   messaggi diversi generati a partire dallo stesso modello.

## Step 4 – Logger generico con vincoli

**File principali:**
- `Step4/IEventoLoggabile.cs`
- `Step4/EventoAccesso.cs`
- `Step4/Logger.cs`
- `Step4/Program.cs`

Punti chiave:
1. La classe `Logger<T>` sfrutta il vincolo `where T : IEventoLoggabile, new()` per
   creare automaticamente le istanze di evento, che vengono poi popolate da una
   lambda di configurazione.
2. Ogni registrazione passa attraverso una **validazione minima** del messaggio
   per evitare log vuoti, ma il punto di estensione è chiaro e facilmente
   ampliabile (ad esempio per controllare la categoria o l'utente).
3. `RecuperaEventi` restituisce una copia degli eventi salvati. Se viene fornito un
   filtro (`Func<T, bool>`), il metodo applica la condizione e ritorna solo gli
   elementi che la soddisfano.
4. Il `Program` di esempio registra tre eventi diversi (successo, fallimento e
   manutenzione) e mostra sia la stampa completa sia un filtro mirato sugli errori.

## Come usare la soluzione

Ogni step è un normale progetto console .NET 6. Per eseguirne uno, spostati nella
relativa cartella e lancia `dotnet run`, ad esempio:

```bash
cd esercizi/LogEventiGuidato/soluzione/Step4
dotnet run
```

Il codice è volutamente ricco di commenti per aiutare lo studente a collegare i
concetti teorici con l'implementazione concreta. Dopo aver consultato questa
cartella puoi tornare ai file in `src/` e riprovare l'esercizio partendo dai TODO.
