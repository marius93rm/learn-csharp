# GestoreMagazzino.TDD

Percorso guidato per esercitarsi con il Test-Driven Development in C#. Ogni milestone introduce un concetto chiave e indica i punti del codice (TODO numerati) da completare. Procedi sempre con il ciclo **Red ➜ Green ➜ Refactor** prima di passare allo step successivo.

## Struttura del progetto

- `src/` contiene il codice di produzione.
- `tests/` contiene i test unitari in xUnit (con supporto a Moq per il mocking).
- Ogni file include TODO numerati che rimandano alle milestone descritte sotto.

Suggerimento: crea una soluzione (`dotnet new sln`) e aggiungi i due progetti (`dotnet sln add ...`) se vuoi lavorare con l'IDE.

## Milestone 1 – Inventario vuoto (SRP)

Obiettivo: modellare un inventario inizialmente vuoto, rispettando il Single Responsibility Principle.

1. Scrivi il test in `tests/InventarioTests.cs` (TODO `1.T1`) che si aspetta un inventario vuoto all'inizio.
2. Implementa la struttura dati dell'inventario in `src/Inventario.cs` (TODO `1.1`).
3. Fai in modo che `ElencoProdotti()` restituisca i prodotti correnti (TODO `1.2`).

Mantieni il design semplice, puntando alla chiarezza del comportamento iniziale.

## Milestone 2 – Aggiunta prodotti (Red-Green)

Consolida il ciclo Red-Green: parti dai test, poi implementa il minimo indispensabile.

1. Aggiungi un nuovo test in `tests/InventarioTests.cs` (TODO `2.T1`) che fallisca fino a quando non potrai aggiungere prodotti.
2. Implementa `AggiungiProdotto` in `src/Inventario.cs` lavorando sui TODO `2.1` e `2.2`.

Ricorda di mantenere il codice pulito dopo il refactor e di verificare sempre i test.

## Milestone 3 – Rimozione e duplicati

Estendi il modello per gestire rimozioni e prevenire duplicati.

1. Scrivi/aggiorna i test dedicati in `tests/InventarioTests.cs` (TODO `3.T1` e `3.T2`).
2. Completa `RimuoviProdotto` in `src/Inventario.cs` agendo sui TODO `3.1`, `3.2` e `3.3`.

Definisci eccezioni esplicite per i casi limite: fallisci il test prima, poi implementa la logica.

## Milestone 4 – Scorte e quantità (Refactor)

Introduce il concetto di quantità per ciascun prodotto e cogli l'occasione per rifattorizzare.

1. Aggiorna i test esistenti o aggiungine di nuovi per le quantità (`tests/InventarioTests.cs`, TODO `4.T1` e `4.T2`).
2. Progetta l'oggetto `Prodotto` in `src/Prodotto.cs` (TODO `4.4` e `4.5`).
3. Implementa la gestione delle scorte in `src/Inventario.cs` (TODO `4.1`, `4.2`, `4.3`).

Valuta se introdurre pattern che facilitino la manutenzione del codice durante il refactoring.

## Milestone 5 – Mock e INotificatore (DIP + Moq)

Integra un canale di notifica quando le scorte scendono sotto una soglia minima, applicando il Dependency Inversion Principle.

1. Definisci l'interfaccia `INotificatoreMagazzino` in `src/INotificatoreMagazzino.cs` (TODO `5.3`).
2. Amplia i test utilizzando Moq (`tests/InventarioTests.cs`, TODO `5.T1` e `5.T2`) per simulare il notificatore.
3. Collega le notifiche alla logica di riduzione scorte in `src/Inventario.cs` (TODO `5.1` e `5.2`).

Sperimenta con Moq per verificare il numero di chiamate e gli argomenti passati al notificatore.

---

### Suggerimenti finali

- Procedi sempre da un test fallente: evita di implementare la logica prima di avere un test.
- Mantieni il focus su una milestone alla volta: refactoring e nuove feature devono essere distinti.
- Documenta gli eventuali ragionamenti o decisioni nelle commit message per seguire la storia TDD.

Buon lavoro e buon TDD!
