# Guida Didattica all'Esercizio sui Microservizi in C#

Benvenuti! In questa guida approfondiremo passo passo l'esercizio sui microservizi
contenuto nella cartella `esercizi/microservices`. L'obiettivo è comprendere non solo
cosa fa ogni componente, ma anche *perché* è stato progettato in un certo modo, con
particolare attenzione ai principi di architettura software applicati. Immagina questa
guida come la spiegazione che riceveresti durante una lezione universitaria: analizzeremo
ogni file, metodo e concetto rilevante, collegandolo ai pattern e ai principi SOLID.

---

## 1. Panoramica del progetto

Il progetto è composto da tre microservizi principali più una libreria condivisa:

- **`Shared/Models.cs`**: definisce i Data Transfer Object (DTO) e le interfacce che
  costituiscono i contratti tra i servizi. È il "vocabolario comune" dell'ecosistema.
- **`UserService/UserService.cs`**: implementa la logica del microservizio utenti,
  responsabile di leggere e validare informazioni sugli utenti.
- **`OrderService/OrderService.cs`**: gestisce la creazione degli ordini, applicando
  regole di dominio e garantendo idempotenza.
- **`Gateway/Gateway.cs`**: funge da facciata verso l'esterno, orchestrando la
  collaborazione tra i servizi e introducendo aspetti di resilienza.

Ogni modulo applica il **Dependency Inversion Principle (DIP)**: dipende da interfacce
(definite in `Shared`) e non dalle implementazioni concrete. Questo approccio consente
una sostituzione trasparente delle dipendenze e rende il codice facilmente testabile.

---

## 2. Roadmap delle milestone

Per facilitare lo studio, la soluzione è stata organizzata in quattro milestone, ognuna
con passi incrementali:

1. **Milestone 1 – UserService pronto per la produzione**
   - Definizione dei contratti utente e logger condiviso.
   - Implementazione del servizio di lettura con validazione e logging.
   - Repository in memoria con gestione duplicati e seed idempotente.

2. **Milestone 2 – OrderService con regole di dominio**
   - DTO degli ordini e contratti repository.
   - Servizio di scrittura con controlli di idempotenza e validazioni.
   - Repository concorrente con indice composto.

3. **Milestone 3 – Gateway orchestratore**
   - Validazione precoce della richiesta lato gateway.
   - Composizione manuale delle dipendenze.
   - Coordinamento tra user service e order service.

4. **Milestone 4 – Resilienza, logging e osservabilità**
   - Risultati operativi uniformi (`OperationResult`).
   - Timeout, retry, gestione errori strutturata e correlazione delle richieste.
   - Simulazione di latenza/reti instabili tramite `HttpClientSimulator`.

Nei paragrafi seguenti esamineremo ogni milestone mostrando il codice commentato e
spiegandone ogni singola parte.

---

## 3. Milestone 1 – UserService

La prima milestone mette le fondamenta dell'ecosistema: ci concentriamo sul servizio
utenti, sul contratto condiviso e sull'infrastruttura minima di logging necessaria a
monitorare il comportamento del microservizio. L'obiettivo didattico è mostrare come
modellare dati immutabili, validare gli ingressi e separare la logica applicativa
dall'accesso ai dati tramite un repository in memoria.

### 3.1 Contratti condivisi e logger di base (`Shared/Models.cs`)

```csharp
// DTO condivisi che descrivono gli oggetti utente e ordine scambiati tra servizi.
public record UserDto(Guid Id, string Email); // Contiene l'identificativo univoco e la mail verificata dell'utente.
public record OrderDto(Guid Id, Guid UserId, string ProductCode, int Quantity, DateTimeOffset CreatedAtUtc); // L'ordine fa riferimento all'utente, al prodotto acquistato e al timestamp di creazione.

// Richiesta di creazione ordine: include un RequestId facoltativo per supportare l'idempotenza.
public record CreateOrderRequest(Guid UserId, string ProductCode, int Quantity, string? RequestId = null); // I dati arrivano dal client esterno e devono essere validati.

// Risposta dal gateway che conferma la creazione dell'ordine.
public record OrderConfirmationDto(Guid OrderId, string Message, DateTimeOffset CreatedAtUtc); // Comunica al client l'esito positivo con il messaggio human-friendly.

// Struttura per comunicare errori standardizzati.
public record OperationError(string Code, string Message); // Ogni errore ha un codice per l'automazione e un messaggio per l'utente.

// Logger minimale che rende il logging sostituibile in ogni microservizio.
public interface IAppLogger
{
    void Info(string message);                         // Log informativi: traccia il flusso nominale.
    void Warning(string message);                      // Log di attenzione: segnalano problemi recuperabili.
    void Error(string message, Exception? exception = null); // Log di errore: includono l'eccezione originale se presente.
}
```

I record offrono immutabilità e uguaglianza strutturale, ideale per DTO. Le interfacce
esposte permettono di applicare DIP: i servizi lavorano su contratti, non su classi
concrete.

### 3.2 Implementare il logger e le eccezioni (`UserService/UserService.cs`)

```csharp
// Logger console thread-safe: ogni microservizio scrive con il proprio "scope".
public class ConsoleLogger : IAppLogger
{
    private readonly string _scope;                     // Identifica il servizio che logga (es. "UserService").
    private static readonly object _sync = new();        // Garantisce scritture atomiche su Console.

    public ConsoleLogger(string scope) => _scope = scope;

    public void Info(string message) => Write("INFO", message);          // Livello informativo.
    public void Warning(string message) => Write("WARN", message);        // Livello di avviso.
    public void Error(string message, Exception? exception = null)
        => Write("ERROR", exception is null ? message : $"{message}: {exception.Message}"); // Integra messaggio eccezione.

    private void Write(string level, string message)
    {
        lock (_sync)                                   // Evita l'interleaving tra thread concorrenti.
        {
            Console.WriteLine($"[{DateTimeOffset.UtcNow:O}] [{level}] [{_scope}] {message}"); // Timestamp ISO 8601 per parsing automatico.
        }
    }
}

// Eccezione dedicata per segnalare errori di validazione lato utente.
public class UserValidationException : Exception
{
    public UserValidationException(string message) : base(message) { }
}
```

Questo logger è utilizzato sia nel `UserService` sia dagli altri componenti, evitando di
ancorare il codice a librerie esterne e permettendo test unitari con logger fittizi.

### 3.3 Servizio di lettura con validazione (`UserReadService`)

```csharp
public class UserReadService : IUserReadService
{
    private readonly IUserRepository _repository;   // Dipendenza dal repository astratto.
    private readonly IAppLogger _logger;            // Logger iniettato per tracciare l'esecuzione.

    public UserReadService(IUserRepository repository, IAppLogger logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)                  // Validazione input: evita richieste malformate.
        {
            const string message = "UserId must be a non-empty GUID.";
            _logger.Warning(message);              // Log a livello warning per segnalare il problema.
            throw new UserValidationException(message);
        }

        _logger.Info($"Reading user {userId}.");
        var user = await _repository.FindByIdAsync(userId, cancellationToken).ConfigureAwait(false); // ConfigureAwait(false) evita deadlock in contesti legacy (es. UI thread).

        if (user is null)
        {
            _logger.Warning($"User {userId} not found.");
        }

        return user;                               // Può essere null: il gateway gestirà il caso.
    }
}
```

Il servizio è responsabile esclusivamente della logica applicativa: validazione input e
interazione con il repository. Non conosce dettagli infrastrutturali.

### 3.4 Repository in memoria (`InMemoryUserRepository`)

```csharp
public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, UserDto> _users = new();          // Archivio principale.
    private readonly ConcurrentDictionary<string, Guid> _indexByEmail =           // Indice secondario per email.
        new(StringComparer.OrdinalIgnoreCase);

    public Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();                           // Propaga immediatamente l'annullamento.
        _users.TryGetValue(userId, out var user);                                 // Restituisce null se mancante.
        return Task.FromResult(user);
    }

    public Task<UserDto?> FindByEmailAsync(string email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();                           // Evita di proseguire se la richiesta è stata annullata.
        if (_indexByEmail.TryGetValue(email, out var id) && _users.TryGetValue(id, out var user))
        {
            return Task.FromResult<UserDto?>(user);                               // Consente ricerche rapide per email.
        }

        return Task.FromResult<UserDto?>(null);
    }

    public Task AddAsync(UserDto user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();                           // Anche le operazioni di scrittura rispettano l'annullamento cooperativo.

        if (string.IsNullOrWhiteSpace(user.Email))                                // Regola di dominio: email obbligatoria.
        {
            throw new UserValidationException("Email must be provided.");
        }

        if (_indexByEmail.ContainsKey(user.Email))                               // Evita duplicati per email.
        {
            throw new UserValidationException($"User with email {user.Email} already exists.");
        }

        if (!_users.TryAdd(user.Id, user))                                       // Evita duplicati per id.
        {
            throw new UserValidationException($"A user with id {user.Id} already exists.");
        }

        if (!_indexByEmail.TryAdd(user.Email, user.Id))                           // Mantiene consistenza tra dizionari.
        {
            _users.TryRemove(user.Id, out _);                                     // Rollback in caso di conflitto.
            throw new UserValidationException($"User with email {user.Email} already exists.");
        }

        return Task.CompletedTask;
    }
}
```

La scelta di `ConcurrentDictionary` rende il repository thread-safe, permettendo di
simulare un contesto multi-thread tipico dei microservizi.

### 3.5 Composizione e seed dati (`UserServiceModule`)

```csharp
public static class UserServiceModule
{
    private static readonly IReadOnlyList<UserDto> _seedUsers = new List<UserDto>
    {
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "demo@example.com"),   // Utente basic pre-caricato.
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "premium@example.com") // Utente premium per scenari avanzati.
    };

    public static IUserReadService CreateUserReadService(
        IUserRepository? repository = null,
        IAppLogger? logger = null)
    {
        repository ??= new InMemoryUserRepository();                              // Default: repository in memoria.
        logger ??= new ConsoleLogger("UserService");                              // Logger contestualizzato.
        SeedSampleData(repository, logger).GetAwaiter().GetResult();               // Seed idempotente: GetAwaiter() evita deadlock rispetto a .Result.
        return new UserReadService(repository, logger);
    }

    private static async Task SeedSampleData(IUserRepository repository, IAppLogger logger)
    {
        foreach (var user in _seedUsers)
        {
            try
            {
                var existing = await repository.FindByIdAsync(user.Id, CancellationToken.None).ConfigureAwait(false); // Usa CancellationToken.None per sottolineare che il seed è opzionale.
                if (existing is null)
                {
                    await repository.AddAsync(user, CancellationToken.None).ConfigureAwait(false); // Inserisce solo se l'utente non esiste già.
                    logger.Info($"Seeded user {user.Email}.");           // Il log conferma l'avvenuta inizializzazione.
                }
            }
            catch (UserValidationException ex)
            {
                logger.Warning($"Skipping seed for {user.Email}: {ex.Message}");  // Idempotenza: seed non fallisce se già presente.
            }
        }
    }
}
```

La factory centralizza la creazione delle dipendenze, anticipando l'introduzione di un
Dependency Injection container più evoluto.

---

## 4. Milestone 2 – OrderService

La seconda milestone amplia lo scenario introducendo il dominio degli ordini: qui
esploriamo come modellare regole di business, garantire l'idempotenza e mantenere la
coerenza tra archivio primario e indice secondario. Il focus didattico è capire come i
servizi di scrittura gestiscono conflitti concorrenti e come lanciare eccezioni specifiche
che il chiamante può distinguere facilmente.

### 4.1 Eccezioni di dominio

```csharp
public class OrderValidationException : Exception
{
    public OrderValidationException(string message) : base(message) { }
}

public class OrderConflictException : Exception
{
    public OrderConflictException(string message) : base(message) { }
}
```

Separa gli errori di validazione (input errato) dai conflitti di concorrenza/idempotenza,
così il chiamante può reagire in modo appropriato.

### 4.2 Servizio di scrittura ordini (`OrderWriteService`)

```csharp
public class OrderWriteService : IOrderWriteService
{
    private static readonly HashSet<string> _catalog = new(StringComparer.OrdinalIgnoreCase)
    {
        "BOOK-123", "COURSE-ADV-C#", "LIC-PREMIUM"      // Catalogo prodotti validi. L'uso del comparer elimina problemi di maiuscole/minuscole.
    };

    private readonly IOrderRepository _repository;
    private readonly IAppLogger _logger;

    public OrderWriteService(IOrderRepository repository, IAppLogger logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        Validate(request);                                                           // Regole di dominio (quantità, prodotto...).
        cancellationToken.ThrowIfCancellationRequested();

        var existing = await _repository                                             // Idempotenza: verifica ordini equivalenti.
            .FindByUserAndProductAsync(request.UserId, request.ProductCode, cancellationToken)
            .ConfigureAwait(false);                                                  // ConfigureAwait(false) per evitare contesti di sincronizzazione.
        if (existing is not null)
        {
            _logger.Info($"Idempotent match for {request.UserId} - {request.ProductCode}. Returning existing order {existing.Id}.");
            return existing;                                                         // Restituisce ordine già creato.
        }

        var order = new OrderDto(Guid.NewGuid(), request.UserId, request.ProductCode, request.Quantity, DateTimeOffset.UtcNow); // Crea l'ordine in memoria con timestamp in UTC.
        try
        {
            await _repository.AddAsync(order, cancellationToken).ConfigureAwait(false);
            _logger.Info($"Created order {order.Id} for user {order.UserId}."); // Log conferma la persistenza riuscita.
        }
        catch (OrderConflictException ex)
        {
            _logger.Warning(ex.Message);                                             // Conflitto concorrente: riprova a leggere l'ordine.
            var duplicated = await _repository.FindByUserAndProductAsync(request.UserId, request.ProductCode, cancellationToken)
                .ConfigureAwait(false);                                             // Il read-after-write consente di recuperare l'ordine appena inserito da un altro nodo.
            if (duplicated is not null)
            {
                return duplicated;                                                   // Recupera ordine inserito da un altro thread.
            }
            throw;                                                                   // Se non recuperabile, propaga l'errore.
        }

        return order;
    }

    private static void Validate(CreateOrderRequest request)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new OrderValidationException("UserId must be a non-empty GUID.");
        }
        if (string.IsNullOrWhiteSpace(request.ProductCode))
        {
            throw new OrderValidationException("ProductCode must be provided.");
        }
        if (!_catalog.Contains(request.ProductCode))
        {
            throw new OrderValidationException($"Product {request.ProductCode} does not exist in the catalog."); // Previene ordini con SKU non supportati.
        }
        if (request.Quantity <= 0)
        {
            throw new OrderValidationException("Quantity must be greater than zero."); // Impedisce ordini nulli o negativi.
        }
    }
}
```

Il servizio isola la logica di dominio: valida la richiesta, controlla l'idempotenza e
interagisce col repository. Ogni responsabilità è singola (SRP).

### 4.3 Repository concorrente (`InMemoryOrderRepository`)

```csharp
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, OrderDto> _orders = new();          // Archivio principale.
    private readonly ConcurrentDictionary<string, Guid> _index =                    // Indice user+product per idempotenza.
        new(StringComparer.OrdinalIgnoreCase);

    private static string ComposeKey(Guid userId, string productCode)
        => $"{userId:N}:{productCode}";                                            // Chiave composta normalizzata. userId:N rimuove i trattini per ridurre la stringa.

    public Task<OrderDto?> FindByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _orders.TryGetValue(orderId, out var order);                                 // Lookup per id ordine.
        return Task.FromResult(order);
    }

    public Task<OrderDto?> FindByUserAndProductAsync(Guid userId, string productCode, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (_index.TryGetValue(ComposeKey(userId, productCode), out var orderId) && _orders.TryGetValue(orderId, out var order))
        {
            return Task.FromResult<OrderDto?>(order);
        }
        return Task.FromResult<OrderDto?>(null);
    }

    public Task AddAsync(OrderDto order, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var key = ComposeKey(order.UserId, order.ProductCode);                       // La chiave combina userId e prodotto per l'idempotenza.

        if (!_orders.TryAdd(order.Id, order))                                       // Rileva conflitti di id ordine.
        {
            throw new OrderConflictException($"Order with id {order.Id} already exists.");
        }

        if (!_index.TryAdd(key, order.Id))                                          // Garantisce unicità per user+product.
        {
            _orders.TryRemove(order.Id, out _);                                     // Rollback se l'indice fallisce.
            throw new OrderConflictException("An equivalent order already exists.");
        }

        return Task.CompletedTask;
    }
}
```

La combinazione di dizionari consente di garantire l'idempotenza, fondamentale quando il
gateway ritenta la creazione di un ordine.

### 4.4 Factory del servizio (`OrderServiceModule`)

```csharp
public static class OrderServiceModule
{
    public static IOrderWriteService CreateOrderWriteService(
        IOrderRepository? repository = null,
        IAppLogger? logger = null)
    {
        repository ??= new InMemoryOrderRepository();                                // Implementazione predefinita.
        logger ??= new ConsoleLogger("OrderService");                               // Riusa il logger del progetto.
        return new OrderWriteService(repository, logger);
    }
}
```

Questo modulo rende semplice sostituire il repository con un database reale durante gli
esperimenti successivi.

---

## 5. Milestone 3 – Gateway orchestratore

La terza milestone costruisce l'orchestratore che coordina i microservizi precedenti:
introduciamo validazioni anticipate, logging arricchito con correlation ID e
interazioni simulate di rete. Lo scopo è comprendere come un gateway centralizza la
logica cross-cutting (retry, timeout, traduzione degli errori) per offrire un'unica API
coerente verso l'esterno.

### 5.1 Gestione delle richieste (`OrderGateway`)

```csharp
public class OrderGateway : IOrderGateway
{
    private readonly IUserReadService _userService;                // Dipendenza dal servizio utenti.
    private readonly IOrderWriteService _orderService;             // Dipendenza dal servizio ordini.
    private readonly IHttpClientSimulator _httpClient;             // Simulatore di rete per inbound/outbound.
    private readonly IAppLogger _logger;                           // Logger contestualizzato.
    private readonly TimeSpan _userServiceTimeout = TimeSpan.FromMilliseconds(500); // Timeout massimo per ogni richiesta verso UserService.
    private readonly int _userServiceMaxAttempts = 3;              // Parametri per i retry verso UserService.

    public OrderGateway(
        IUserReadService userService,
        IOrderWriteService orderService,
        IHttpClientSimulator httpClient,
        IAppLogger logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<OperationResult<OrderConfirmationDto>> CreateOrderAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            ValidateRequest(request);                             // Validazione precoce lato gateway (Milestone 3).
        }
        catch (OrderValidationException ex)
        {
            return OperationResult<OrderConfirmationDto>.Failure("gateway.validation", ex.Message);
        }

        var correlationId = Guid.NewGuid().ToString("N");         // Correlation ID per tracciare l'intero flusso.
        using var scope = new CorrelationScope(correlationId);      // Scope disposable che ripristina automaticamente il vecchio correlation ID.
        _logger.Info($"[{correlationId}] Received request to create order for user {request.UserId}.");

        try
        {
            await _httpClient.SimulateIncomingRequestAsync(correlationId, cancellationToken).ConfigureAwait(false); // Simula la latenza iniziale della richiesta.
        }
        catch (OperationCanceledException)
        {
            return OperationResult<OrderConfirmationDto>.Failure("gateway.canceled", "Request was cancelled by the caller.");
        }

        UserDto user;                                              // Conterrà l'utente validato recuperato dal servizio remoto.
        try
        {
            user = await FetchUserWithRetryAsync(request.UserId, cancellationToken).ConfigureAwait(false);
        }
        catch (UserValidationException ex)
        {
            return OperationResult<OrderConfirmationDto>.Failure("user.validation", ex.Message);
        }
        catch (TimeoutException ex)
        {
            _logger.Error($"[{correlationId}] Timeout while retrieving user {request.UserId}.", ex);
            return OperationResult<OrderConfirmationDto>.Failure("user.timeout", "User service is not responding. Please retry later.");
        }
        catch (Exception ex)
        {
            _logger.Error($"[{correlationId}] Unexpected error while retrieving user {request.UserId}.", ex);
            return OperationResult<OrderConfirmationDto>.Failure("user.unexpected", "An unexpected error occurred while retrieving the user.");
        }

        if (user is null)
        {
            _logger.Warning($"[{correlationId}] User {request.UserId} not found.");
            return OperationResult<OrderConfirmationDto>.Failure("user.not_found", "The specified user does not exist.");
        }

        OrderDto order;                                            // Risultato restituito da OrderService.
        try
        {
            order = await _orderService.CreateOrderAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (OrderValidationException ex)
        {
            _logger.Warning($"[{correlationId}] Validation error from OrderService: {ex.Message}");
            return OperationResult<OrderConfirmationDto>.Failure("order.validation", ex.Message);
        }
        catch (OrderConflictException ex)
        {
            _logger.Warning($"[{correlationId}] Conflict detected in OrderService: {ex.Message}");
            return OperationResult<OrderConfirmationDto>.Failure("order.conflict", ex.Message);
        }
        catch (OperationCanceledException)
        {
            return OperationResult<OrderConfirmationDto>.Failure("order.canceled", "Order creation cancelled.");
        }
        catch (Exception ex)
        {
            _logger.Error($"[{correlationId}] Unexpected error from OrderService.", ex);
            return OperationResult<OrderConfirmationDto>.Failure("order.unexpected", "An unexpected error occurred while creating the order.");
        }

        await _httpClient.SimulateOutgoingCallAsync("OrderService", correlationId, cancellationToken).ConfigureAwait(false);

        var confirmation = new OrderConfirmationDto(order.Id, $"Order created for {user.Email}", order.CreatedAtUtc);
        _logger.Info($"[{correlationId}] Order {order.Id} created successfully for user {user.Email}.");
        return OperationResult<OrderConfirmationDto>.Success(confirmation);
    }

    private static void ValidateRequest(CreateOrderRequest request)
    {
        if (request is null)
        {
            throw new OrderValidationException("Request cannot be null.");
        }
        if (string.IsNullOrWhiteSpace(request.ProductCode))
        {
            throw new OrderValidationException("ProductCode must be provided."); // Il gateway fa da prima linea di difesa.
        }
        if (request.Quantity <= 0)
        {
            throw new OrderValidationException("Quantity must be greater than zero.");
        }
        if (request.UserId == Guid.Empty)
        {
            throw new OrderValidationException("UserId must be a non-empty GUID.");
        }
    }
}
```

Il gateway centralizza la gestione degli errori restituendo `OperationResult`, così il
client riceve sempre risposte coerenti. Le validazioni anticipate evitano di propagare
richieste invalide ai servizi a valle.

### 5.2 Retry con timeout (`FetchUserWithRetryAsync`)

```csharp
private async Task<UserDto?> FetchUserWithRetryAsync(Guid userId, CancellationToken cancellationToken)
{
    var exceptions = new List<Exception>();                       // Accumula gli errori per diagnosticare eventuali fallimenti ripetuti.
    var attempt = 0;                                               // Contatore dei tentativi già eseguiti.

    while (attempt < _userServiceMaxAttempts)
    {
        cancellationToken.ThrowIfCancellationRequested();
        attempt++;
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken); // Combina il token chiamante con uno specifico per il timeout.
        timeoutCts.CancelAfter(_userServiceTimeout);                      // Timeout dedicato per ogni chiamata.

        try
        {
            await _httpClient.SimulateOutgoingCallAsync(
                "UserService",
                CorrelationScope.CurrentId ?? string.Empty,
                timeoutCts.Token).ConfigureAwait(false);

            return await _userService.GetUserAsync(userId, timeoutCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            if (timeoutCts.IsCancellationRequested)
            {
                exceptions.Add(new TimeoutException("User service call timed out.", ex));
                _logger.Warning($"Timeout while calling UserService (attempt {attempt}).");
            }
            else
            {
                throw;                                                     // Annullamento esplicito del chiamante.
            }
        }
        catch (Exception ex) when (attempt < _userServiceMaxAttempts)
        {
            exceptions.Add(ex);
            _logger.Warning($"Transient error while calling UserService (attempt {attempt}): {ex.Message}");
            await Task.Delay(TimeSpan.FromMilliseconds(100 * attempt), cancellationToken).ConfigureAwait(false); // Backoff lineare crescente fra un tentativo e l'altro.
        }
    }

    throw new TimeoutException("Unable to reach the user service after multiple attempts.",
        exceptions.Count > 0 ? exceptions[^1] : null);
}
```

Questa routine implementa politiche di resilienza: tentativi multipli, timeout per singola
chiamata e backoff incrementale.

### 5.3 Simulatore HTTP e correlazione (`HttpClientSimulator`, `CorrelationScope`)

```csharp
public interface IHttpClientSimulator
{
    Task SimulateIncomingRequestAsync(string correlationId, CancellationToken cancellationToken);
    Task SimulateOutgoingCallAsync(string endpointName, string correlationId, CancellationToken cancellationToken);
}

public class HttpClientSimulator : IHttpClientSimulator
{
    private readonly TimeSpan _latency;                           // Latenza media artificiale per ogni chiamata.
    private readonly double _failureProbability;                  // Probabilità di generare un errore transiente.
    private readonly Random _random = new();                      // RNG per determinare i fallimenti casuali.

    public HttpClientSimulator(TimeSpan? latency = null, double failureProbability = 0.0)
    {
        _latency = latency ?? TimeSpan.FromMilliseconds(60);          // Latenza di default.
        _failureProbability = Math.Clamp(failureProbability, 0, 1);   // Percentuale di errori transienti.
    }

    public async Task SimulateIncomingRequestAsync(string correlationId, CancellationToken cancellationToken)
    {
        await Task.Delay(_latency, cancellationToken).ConfigureAwait(false);  // Ritardo per simulare arrivo richieste.
    }

    public async Task SimulateOutgoingCallAsync(string endpointName, string correlationId, CancellationToken cancellationToken)
    {
        await Task.Delay(_latency, cancellationToken).ConfigureAwait(false);  // Ritardo sulle chiamate in uscita.

        if (_failureProbability > 0 && _random.NextDouble() < _failureProbability)
        {
            throw new InvalidOperationException($"Transient error contacting {endpointName}.");  // Errore casuale controllato.
        }
    }
}

public sealed class CorrelationScope : IDisposable
{
    private static readonly AsyncLocal<string?> _currentId = new();             // Conserva il correlationId nel contesto async.
    private readonly string? _previousId;                                       // Memorizza l'ID corrente per ripristinarlo in Dispose.

    public CorrelationScope(string correlationId)
    {
        _previousId = _currentId.Value;                                         // Salva l'ID precedente.
        _currentId.Value = correlationId;                                       // Imposta l'ID per il nuovo scope.
    }

    public static string? CurrentId => _currentId.Value;                        // Espone l'ID attivo ai componenti che fanno logging.

    public void Dispose()
    {
        _currentId.Value = _previousId;                                         // Ripristina l'ID precedente.
    }
}
```

Il simulatore consente di introdurre condizioni di rete variabili, mentre `CorrelationScope`
propaga un identificatore lungo la catena di chiamate per log leggibili.

### 5.4 Composizione (`GatewayModule`)

```csharp
public static class GatewayModule
{
    public static IOrderGateway CreateOrderGateway(
        IUserReadService? userService = null,
        IOrderWriteService? orderService = null,
        IHttpClientSimulator? httpClient = null,
        IAppLogger? logger = null)
    {
        userService ??= UserServiceModule.CreateUserReadService();
        orderService ??= OrderServiceModule.CreateOrderWriteService();
        httpClient ??= new HttpClientSimulator();                 // Simulatore con valori di default.
        logger ??= new ConsoleLogger("Gateway");                  // Logger dedicato all'orchestratore.
        return new OrderGateway(userService, orderService, httpClient, logger);
    }
}
```

Il gateway viene cablato manualmente per mostrare come ogni servizio dipenda solo da
interfacce. In un contesto reale, questo codice verrebbe sostituito da un container DI.

---

## 6. Milestone 4 – Risultati uniformi e gestione errori

Nell'ultima milestone si consolida l'esperienza dell'utente finale: formalizziamo un
contratto di risposta uniforme che incapsula dati ed errori, così da semplificare
l'integrazione con i client. L'accento è sulla resilienza a livello comunicativo e sulla
tracciabilità degli errori grazie a codici standardizzati.

Questa milestone introduce la record `OperationResult<T>` nel progetto condiviso:

```csharp
public record OperationResult<T>(T? Value, OperationError? Error)
{
    public bool IsSuccess => Error is null;                                       // Comodo flag per verificare l'esito.
    public static OperationResult<T> Success(T value) => new(value, null);       // Factory per successi.
    public static OperationResult<T> Failure(string code, string message)        // Factory per errori.
        => new(default, new OperationError(code, message));                      // Value è default perché l'operazione è fallita.
}
```

Grazie a questa struttura, il gateway può comunicare al client sia il risultato (ordine
creato) sia errori strutturati con un codice e un messaggio comprensibile.

---

## 7. Collegamenti ai principi architetturali

- **Single Responsibility Principle (SRP)**: ogni classe ha un compito ben definito
  (es. `OrderWriteService` gestisce solo la logica di creazione ordini).
- **Open/Closed Principle (OCP)**: i servizi dipendono da interfacce; puoi estenderli
  creando nuove implementazioni (es. repository su database) senza modificarne il codice.
- **Dependency Inversion Principle (DIP)**: le dipendenze sono invertite rispetto ai
  dettagli concreti. Tutti i servizi si appoggiano ai contratti definiti in `Shared`.
- **Repository Pattern**: i repository in memoria isolano l'accesso ai dati.
- **API Gateway Pattern**: `OrderGateway` incapsula l'orchestrazione delle chiamate.
- **Idempotenza**: il repository ordini garantisce che richieste duplicate non producano
  effetti collaterali.
- **Resilienza**: retry, timeout e simulazione di fallimenti mostrano come proteggere il
  sistema da servizi lenti o non disponibili.

---

## 8. Flusso di esecuzione completo

1. Il client (simulato) invia una richiesta di creazione ordine al Gateway.
2. Il Gateway applica validazione preliminare e registra un `correlationId`.
3. Viene simulata la latenza della richiesta in ingresso.
4. Il Gateway interroga `UserService` con politiche di retry e timeout.
5. Se l'utente esiste, viene invocato `OrderService` per creare l'ordine in modo idempotente.
6. Il Gateway simula la chiamata in uscita e costruisce un `OrderConfirmationDto`.
7. L'esito viene incapsulato in `OperationResult` e restituito al client.

Questo flusso dimostra come i microservizi collaborano tramite contratti condivisi,
mantenendo indipendenza e possibilità di scalare/separare il deploy.

---

## 9. Suggerimenti per esercizi (TODO)

I TODO numerati all'interno del codice rappresentano estensioni didattiche. Alcune idee:

1. Sostituire i repository in memoria con implementazioni basate su database o file.
2. Introdurre un sistema di logging strutturato (es. Serilog) senza toccare i servizi.
3. Implementare policy di retry personalizzate con exponential backoff nel Gateway.
4. Ampliare il catalogo prodotti recuperandolo da una fonte esterna tramite un nuovo
   microservizio.
5. Aggiungere metriche e tracing distribuito sfruttando `CorrelationScope`.
6. Scrivere test unitari per ogni servizio, sfruttando le interfacce per fare mocking.
7. Integrare un Circuit Breaker nel Gateway per isolare i servizi in errore.
8. Gestire RequestId idempotenti nel servizio ordini (magari introducendo una tabella di
   deduplicazione separata).

Ogni TODO è un punto di partenza per esercitarsi su aspetti avanzati: resilienza,
osservabilità, configurabilità.

---

## 10. Come proseguire

Per consolidare l'apprendimento, puoi:

- Estendere i contratti condivisi per supportare nuovi casi d'uso (es. annullamento ordini).
- Automatizzare la composizione con un container DI (es. Microsoft.Extensions.DependencyInjection).
- Simulare carichi concorrenti per osservare il comportamento dei repository.
- Documentare le API esposte dal Gateway in formato OpenAPI per generare client automatici.

Buon lavoro! Ricorda: l'obiettivo non è solo far funzionare il codice, ma comprendere come
i microservizi comunicano e come l'architettura favorisce l'evoluzione indipendente dei
componenti.
