# Soluzione esercizio Microservices

Questa cartella contiene una possibile implementazione completa dei microservizi
richiesti dall'esercizio. L'obiettivo è mostrare come applicare principi di
separazione delle responsabilità, validazione del dominio e resilienza alla
comunicazione tra servizi.

## Struttura

- `Shared/Models.cs`: definisce i DTO condivisi, le interfacce dei servizi e un
  semplice `OperationResult<T>` per propagare errori strutturati. Include anche
  l'interfaccia `IAppLogger` usata per disaccoppiare il logging dalle
  implementazioni.
- `UserService/UserService.cs`: implementa un microservizio di lettura utenti con
  repository in memoria, validazioni e seeding idempotente di dati di esempio.
- `OrderService/OrderService.cs`: contiene la logica di creazione ordine con
  validazione del dominio, controllo di idempotenza e repository concorrente in
  memoria.
- `Gateway/Gateway.cs`: orchestra i microservizi tramite un gateway che applica
  retry con timeout verso lo user service, restituisce errori strutturati e
  aggiunge un simulatore di client HTTP con supporto a correlation id.

## Come usare la soluzione

Il modulo `GatewayModule.CreateOrderGateway()` compone automaticamente le
implementazioni concrete. È possibile personalizzare repository, logger e
simulatore HTTP passando implementazioni alternative al factory method.

Esempio di utilizzo in un test o in un'applicazione console:

```csharp
var gateway = GatewayModule.CreateOrderGateway();
var request = new CreateOrderRequest(
    Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
    "BOOK-123",
    1,
    "client-request-001");

var result = await gateway.CreateOrderAsync(request, CancellationToken.None);

if (result.IsSuccess)
{
    Console.WriteLine($"Ordine creato: {result.Value!.OrderId}");
}
else
{
    Console.WriteLine($"Errore: {result.Error!.Code} - {result.Error.Message}");
}
```

Questa implementazione privilegia la chiarezza rispetto alla completezza
funzionale, ma fornisce un punto di partenza per esplorare estensioni come
persistenza su database, orchestrazione asincrona e strumenti di osservabilità.

## Commento dettagliato sulle milestone

Per aiutarti a collegare la soluzione finale con il percorso guidato
nell'esercizio, di seguito trovi un riepilogo passo passo di come ogni
milestone proposta nel file `README.md` principale viene soddisfatta dal
codice in questa cartella.

### Milestone 1 – Creare UserService

1. **Definizione contratti e modelli** – In `Shared/Models.cs` sono presenti i
   DTO `UserDto` e le interfacce `IUserRepository` e `IUserReadService`.
   Queste astrazioni permettono al servizio di conoscere solo il contratto e
   non l'implementazione concreta del repository.
2. **Repository in memoria** – `UserService/UserService.cs` implementa
   `InMemoryUserRepository`, una struttura thread-safe basata su
   `ConcurrentDictionary`. Il metodo `TryAdd` previene duplicazioni e garantisce
   l'idempotenza del seeding.
3. **Validazione d'ingresso** – Il metodo `GetByIdAsync` effettua controlli su
   `Guid.Empty` e registra errori tramite `IAppLogger`. Gli errori vengono
   restituiti con `OperationResult<UserDto>` per mantenere la segnalazione
   coerente con il resto del sistema.
4. **Seeding dei dati** – La factory `UserServiceModule.CreateUserService`
   popola il repository con utenti di esempio. Il seeding è idempotente, così la
   milestone resta valida anche in scenari di bootstrap ripetuto.

### Milestone 2 – Integrare OrderService

1. **Contratti del dominio ordini** – `Shared/Models.cs` ospita `CreateOrderRequest`,
   `OrderDto`, `IOrderRepository` e `IOrderCommandService`, fornendo un contratto
   coerente per la creazione degli ordini e l'accesso ai dati.
2. **Repository concorrente** – In `OrderService/OrderService.cs` trovi
   `InMemoryOrderRepository`, basato su `ConcurrentDictionary` per gestire più
   richieste concorrenti e assicurare il controllo dei duplicati tramite la
   chiave composta (`UserId`, `ProductSku`).
3. **Logica di business** – `OrderCommandService.CreateAsync` valida quantità e
   SKU, verifica l'esistenza di ordini duplicati, genera un nuovo identificativo
   e restituisce `OrderDto` in caso di successo.
4. **Integrazione con logger** – Tutti i passaggi cruciali sono tracciati con
   `IAppLogger` per supportare troubleshooting e audit.

### Milestone 3 – Creare Gateway

1. **Composizione delle dipendenze** – `Gateway/Gateway.cs` espone
   `GatewayModule.CreateOrderGateway`, che istanzia i servizi concreti e li
   combina applicando manualmente Dependency Injection.
2. **Orchestrazione CreateOrder** – Il metodo `CreateOrderAsync` del gateway
   invoca `IUserReadService` e `IOrderCommandService` nell'ordine corretto,
   propagando errori con `OperationResult<OrderConfirmationDto>`.
3. **Mappatura DTO e risposta** – In caso di successo, il gateway costruisce un
   `OrderConfirmationDto` arricchito con i dati dell'utente per restituire una
   risposta completa al client.

### Milestone 4 – Gestire errori e simulare timeout

1. **Retry e timeout** – Il metodo privato `FetchUserWithRetryAsync` nel gateway
   esegue fino a tre tentativi verso lo user service applicando un timeout per
   ogni chiamata, simulando gli scenari di resilienza richiesti.
2. **Error handling strutturato** – Tutti i servizi propagano errori tramite
   `OperationResult<T>` con codici dedicati (es. `user.validation`,
   `order.conflict`), consentendo al gateway di comunicare messaggi coerenti al
   client.
3. **Correlation ID e logging** – `CorrelationScope` e `HttpClientSimulator`
   aggiungono il `correlationId` ai log e riproducono latenza e fault controllati
   per allenarsi al tracciamento delle richieste.
4. **Fallback e short-circuit** – Quando lo user service continua a fallire dopo
   i retry, il gateway interrompe il flusso di creazione ordine restituendo
   immediatamente l'errore senza coinvolgere l'order service, isolando i
   sottosistemi come richiesto dalla milestone.
