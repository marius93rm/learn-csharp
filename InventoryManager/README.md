# Inventory Manager – LINQ avanzato e filtri multi-criterio in C#

Questo mini–progetto propone un gestionale di inventario interamente in memoria pensato per esercitare LINQ e la progettazione di filtri multi-criterio. Troverai uno scheletro completo e compilabile con .NET 8.0: i TODO guidati ti accompagneranno nell'implementazione della logica di filtraggio, dell'ordinamento, della paginazione e delle statistiche. L'obiettivo è comprendere come costruire pipeline di query componibili e facilmente estendibili, simulando uno scenario reale ma a complessità controllata.

## Obiettivi didattici

* Applicare la sintassi method di LINQ (`Where`, `Select`, `OrderBy`, `GroupBy`, `Sum`, `Average`, ecc.).
* Implementare filtri multi-criterio tramite un oggetto di opzioni dedicato (`ProductFilterOptions`).
* Comprendere la deferred execution e quando materializzare una query con `ToList()`.
* Costruire statistiche aggregate con `GroupBy` e proiezioni personalizzate.
* Mantenere un design pulito separando responsabilità (servizio, modello, renderer console).

## Requisiti tecnici

* **SDK richiesto:** .NET 8.0.
* **Tipo di progetto:** console app (`dotnet` CLI o Visual Studio sono entrambi adatti).
* **Come compilare:**
  ```bash
  cd InventoryManager/src/InventoryManager.App
  dotnet build
  ```
* **Come eseguire:**
  ```bash
  cd InventoryManager/src/InventoryManager.App
  dotnet run
  ```
* Assicurati di avere installato l'SDK corretto (`dotnet --version`). Se lavori da IDE, importa la solution `InventoryManager.sln` dalla radice del progetto.

## Struttura del progetto

```
InventoryManager/
├── InventoryManager.sln
├── README.md
└── src/
    └── InventoryManager.App/
        ├── InventoryManager.App.csproj
        ├── Program.cs
        ├── Models/
        │   ├── Product.cs
        │   ├── Category.cs
        │   └── ProductFilterOptions.cs
        ├── Services/
        │   └── InventoryService.cs
        └── Utilities/
            └── ConsoleRenderer.cs
```

* `Models/Product.cs`: definisce la classe `Product` con tutte le proprietà richieste (Id, Name, Category, Price, StockQuantity, IsActive, CreatedAt).
* `Models/Category.cs`: enum con le categorie merceologiche base (puoi estenderla in qualsiasi momento).
* `Models/ProductFilterOptions.cs`: raccoglie i criteri di filtro, l'ordinamento e la paginazione.
* `Services/InventoryService.cs`: ospita i dati seed e la logica LINQ per filtri e statistiche.
* `Utilities/ConsoleRenderer.cs`: si occupa della stampa tabellare in console e dei report testuali.
* `Program.cs`: entrypoint della console app; collega input utente, servizio e renderer.

## Concetti teorici chiave

### Perché usare LINQ

LINQ (Language Integrated Query) consente di descrivere query su collezioni in modo dichiarativo e leggibile. Invece di scrivere `foreach` nidificati, componi operatori come `Where` o `OrderBy`, ottenendo pipeline facili da estendere. Ad esempio, una prima bozza di filtro potrebbe apparire così:

```csharp
IEnumerable<Product> query = _products;

if (filter.OnlyActive)
{
    query = query.Where(p => p.IsActive);
}

// Il resto dei filtri verrà aggiunto milestone dopo milestone.
```

### Oggetto filtro multi-criterio

Invece di passare una dozzina di parametri al metodo `GetProducts`, il progetto usa `ProductFilterOptions`. Questo approccio scala meglio: quando aggiungi un nuovo criterio, devi solo espandere la classe e aggiornare la logica di filtraggio.

```csharp
var filter = new ProductFilterOptions
{
    SearchText = "router",
    Category = Category.Elettronica,
    MinPrice = 100m,
    OnlyActive = true
};
```

### Pipeline step-by-step

Il codice costruisce la query incrementando gradualmente i filtri. Questo pattern è molto leggibile e favorisce l'uso di `IEnumerable<T>`:

```csharp
IEnumerable<Product> query = _products;

if (filter.OnlyActive)
{
    query = query.Where(p => p.IsActive);
}

// TODO Milestone 2 e 3 aggiungeranno qui altri filtri (testo, categoria, range di prezzo, stock...).

query = ApplyOrdering(query, filter);
// TODO: applicare Skip/Take per la paginazione.
```

### Deferred execution

Le query LINQ su `IEnumerable<T>` non vengono eseguite immediatamente: ogni `Where` restituisce una nuova sequenza che verrà valutata solo quando enumerata (ad esempio con `foreach` o `ToList()`). Per questo il metodo `GetProducts` termina con `return query.ToList();`: in questo modo materializzi una fotografia coerente, evitando che modifiche successive alla lista `_products` influenzino i risultati già ottenuti.

## Milestones guidate

### Milestone 1 – Modello e dati di esempio

* **Obiettivo:** comprendere il modello dominio (`Product`, `Category`) e gestire una `List<Product>`.
* **File coinvolti:** `Models/Product.cs`, `Models/Category.cs`, `Services/InventoryService.cs`.
* **TODO da cercare:** `TODO Milestone 1` nel metodo `SeedProducts`.
* **Indicazioni:** aggiungi prodotti con categorie diverse o crea nuove categorie nell'enum. In un contesto reale l'inventario cresce rapidamente: esercitati a mantenere l'ordine e la consistenza dei dati seed.

### Milestone 2 – Filtri base con LINQ (Where)

* **Obiettivo:** implementare filtri per testo e categoria.
* **File coinvolti:** `Models/ProductFilterOptions.cs`, `Services/InventoryService.cs`.
* **TODO da cercare:** `TODO Milestone 2` dentro `GetProducts`.
* **Suggerimenti:** usa `Where` con una lambda. Per la ricerca testo, confronta `p.Name` e `filter.SearchText` in minuscolo (`ToLowerInvariant`). Per la categoria, controlla `filter.Category.HasValue` e filtra i prodotti con la stessa categoria.

### Milestone 3 – Filtri avanzati + Ordinamento + Paginazione

* **Obiettivo:** completare i range di prezzo e stock, impostare l'ordinamento dinamico e la paginazione.
* **File coinvolti:** `Services/InventoryService.cs` (metodo `GetProducts` e `ApplyOrdering`).
* **TODO da cercare:** `TODO Milestone 3`.
* **Suggerimenti:**
  * Per i prezzi: `if (filter.MinPrice.HasValue) query = query.Where(p => p.Price >= filter.MinPrice.Value);` (e simile per `MaxPrice`).
  * Per lo stock: stesso pattern con `StockQuantity`.
  * Ordinamento: nel metodo `ApplyOrdering` valuta `filter.OrderBy` (es. "Name", "Price", "CreatedAt"). Scegli `OrderBy` o `OrderByDescending` a seconda di `filter.OrderDescending`, poi imposta `ThenBy(p => p.Id)` per garantire un ordine stabile.
  * Paginazione: calcola `skip = (filter.Page - 1) * filter.PageSize`, quindi `query = query.Skip(skip).Take(filter.PageSize)`.

### Milestone 4 – Statistiche con LINQ (GroupBy, Sum, Average)

* **Obiettivo:** calcolare il valore totale di stock e un report per categoria.
* **File coinvolti:** `Services/InventoryService.cs` (metodi `GetTotalStockValue` e `GetCategoryStats`).
* **TODO da cercare:** `TODO Milestone 4`.
* **Suggerimenti:**
  * Valore stock: filtra i prodotti attivi, quindi `Sum(p => p.Price * p.StockQuantity)`.
  * Statistiche categoria: `GroupBy(p => p.Category)` e per ogni gruppo calcola `Count()` e `Average(p => p.Price)`. Restituisci la proiezione come tupla.

### Milestone 5 – Console UI e miglioramento design

* **Obiettivo:** integrare input utente, filtri e presentazione a console.
* **File coinvolti:** `Program.cs`, `Utilities/ConsoleRenderer.cs`.
* **TODO da cercare:** `TODO Milestone 5`.
* **Suggerimenti:**
  * Amplia il menu con nuove opzioni (es. esportazione CSV, gestione pagine successive).
  * Leggi dalla console i vari parametri e popola `ProductFilterOptions` prima di chiamare `GetProducts`.
  * Migliora la tabella: potresti aggiungere colori (`Console.ForegroundColor`) o colonne dinamiche.

## Come estendere il progetto

* Leggere i prodotti da file JSON/CSV o da un database (Entity Framework in-memory per prototipi rapidi).
* Gestire filtri multipli per categoria (es. lista di categorie selezionate).
* Esporre la logica come API REST usando ASP.NET Core minimal API.
* Aggiungere test automatici (xUnit, NUnit) per verificare filtri e statistiche.
* Integrare validazioni sugli input e gestione di errori a console.

## Errori comuni

* Dimenticare di chiamare `ToList()` prima di restituire i risultati: questo può portare a riesecuzioni indesiderate della query.
* Applicare `Skip/Take` prima di definire un ordinamento: la paginazione senza ordine stabile produce risultati incoerenti.
* Confondere `First()` con `FirstOrDefault()`: il primo lancia un'eccezione se la sequenza è vuota, il secondo restituisce `null` (o il default del tipo).
* Non normalizzare il testo durante la ricerca: le maiuscole/minuscole possono portare a filtri apparentemente "non funzionanti".
* Ignorare i valori `null` nelle opzioni di filtro: verifica sempre `HasValue` o usa pattern matching (`if (filter.MinPrice is decimal minPrice)`).

Buon lavoro! Procedi milestone per milestone, studia i commenti nel codice e sperimenta piccole variazioni per capire come cambia il comportamento delle query LINQ.
