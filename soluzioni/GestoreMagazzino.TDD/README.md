# GestoreMagazzino.TDD – Soluzione completa

Questa cartella contiene una possibile implementazione completa dell'esercizio TDD "GestoreMagazzino" presente nella cartella `esercizi/`. L'obiettivo è offrire un riferimento pronto all'uso che mostri il risultato finale dopo aver seguito tutte le milestone suggerite nel percorso guidato.

## Struttura della soluzione

```
soluzioni/
  GestoreMagazzino.TDD/
    GestoreMagazzino.TDD.sln
    src/
      GestoreMagazzino.csproj
      Inventario.cs
      Prodotto.cs
      INotificatoreMagazzino.cs
    tests/
      GestoreMagazzino.Tests.csproj
      InventarioTests.cs
```

- **`GestoreMagazzino.TDD.sln`**: soluzione Visual Studio che include il progetto di libreria (`src`) e quello di test (`tests`).
- **`src/`**: codice di produzione, ampiamente commentato per spiegare ogni decisione progettuale.
- **`tests/`**: suite di test xUnit (con Moq) che ripercorre passo passo il cammino TDD delle milestone.

## Come riprodurre i passi TDD

Ogni milestone introdotta nell'esercizio originale è stata tradotta in test e relativo codice di produzione:

1. **Inventario vuoto** – Il test `InventarioIniziaVuoto` verifica che appena creato l'inventario non contenga prodotti. L'implementazione iniziale si limita a restituire una collezione vuota protetta dall'incapsulamento.
2. **Aggiunta prodotti** – Il test `AggiuntaProdottoRendeVisibileIlProdotto` guida la creazione del metodo `AggiungiProdotto`, che istanzia l'oggetto `Prodotto` con validazioni puntuali e lo registra nel dizionario interno.
3. **Duplicati e rimozione** – Il test `NonPermetteDuplicatiERimuoveProdotti` obbliga a gestire le eccezioni per i codici duplicati e la rimozione di prodotti inesistenti. L'uso di eccezioni specifiche rende espliciti gli errori di dominio.
4. **Gestione scorte** – Il test `GestisceScorteEAggiornamentiDiQuantita` verifica l'incremento/decremento delle quantità. La logica è incapsulata nella classe `Prodotto`, che espone metodi `IncrementaQuantita` e `DecrementaQuantita` con controlli rigorosi sui parametri.
5. **Notifiche e DIP** – L'ultimo test `NotificaQuandoLaScortaScendeSottoLaSoglia` usa Moq per assicurarsi che l'inventario collabori con l'interfaccia `INotificatoreMagazzino` quando la scorta scende sotto una soglia. L'inventario non dipende da implementazioni concrete, rispettando il principio di inversione delle dipendenze.

Seguendo i test in ordine, è possibile osservare il ciclo **Red ➜ Green ➜ Refactor** e comprendere come ogni modifica sia stata introdotta guidata da un fallimento iniziale.

## Scelte progettuali principali

- **Dizionario case-insensitive**: l'inventario usa `Dictionary<string, Prodotto>` con `StringComparer.OrdinalIgnoreCase` per evitare duplicati che differiscono solo per maiuscole/minuscole.
- **Oggetto `Prodotto` minimale ma robusto**: codice e descrizione sono immutabili, mentre la quantità viene modificata tramite metodi che centralizzano la validazione.
- **Notifica condizionata**: la notifica scatta solo nel momento in cui si supera la soglia (da sopra a sotto soglia), evitando chiamate ripetute quando si è già sotto il limite.
- **Commenti esaustivi**: ogni metodo contiene note che spiegano il "perché" delle operazioni, in modo da fungere da documentazione viva del percorso.

## Esecuzione dei test

Con l'ambiente .NET installato è sufficiente posizionarsi nella cartella della soluzione e lanciare:

```bash
dotnet test GestoreMagazzino.TDD.sln
```

Il comando ripristinerà i pacchetti (xUnit, Moq, coverlet) e verificherà tutti gli scenari descritti sopra.

## Come riutilizzare la soluzione

- Copia la cartella `soluzioni/GestoreMagazzino.TDD` accanto all'esercizio originale per confrontare passo dopo passo la tua implementazione con quella proposta.
- Utilizza i commenti come guida per spiegare l'esercizio durante workshop o lezioni frontali: ogni blocco di codice richiama le milestone TDD.
- Personalizza i messaggi di eccezione o aggiungi nuove funzionalità partendo dai test già presenti, espandendo ulteriormente la copertura.

Buon studio!
