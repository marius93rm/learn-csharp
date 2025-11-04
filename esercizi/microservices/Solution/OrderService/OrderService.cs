using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Solution.Shared;

namespace Microservices.Solution.OrderService
{
    public class OrderValidationException : Exception
    {
        public OrderValidationException(string message) : base(message)
        {
        }
    }

    public class OrderConflictException : Exception
    {
        public OrderConflictException(string message) : base(message)
        {
        }
    }

    // Milestone 2 - Step 3: servizio di comando per creare ordini applicando regole
    // di dominio e controlli di idempotenza.
    public class OrderWriteService : IOrderWriteService
    {
        private static readonly HashSet<string> _catalog = new(StringComparer.OrdinalIgnoreCase)
        {
            "BOOK-123",
            "COURSE-ADV-C#",
            "LIC-PREMIUM"
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
            Validate(request);
            cancellationToken.ThrowIfCancellationRequested();

            // Milestone 2 - Step 2: il repository fornisce un controllo dedicato per
            // verificare ordini duplicati (idempotenza).
            var existing = await _repository.FindByUserAndProductAsync(request.UserId, request.ProductCode, cancellationToken).ConfigureAwait(false);
            if (existing is not null)
            {
                _logger.Info($"Idempotent match for {request.UserId} - {request.ProductCode}. Returning existing order {existing.Id}.");
                return existing;
            }

            var order = new OrderDto(Guid.NewGuid(), request.UserId, request.ProductCode, request.Quantity, DateTimeOffset.UtcNow);
            try
            {
                await _repository.AddAsync(order, cancellationToken).ConfigureAwait(false);
                _logger.Info($"Created order {order.Id} for user {order.UserId}.");
            }
            catch (OrderConflictException ex)
            {
                _logger.Warning(ex.Message);
                var duplicated = await _repository.FindByUserAndProductAsync(request.UserId, request.ProductCode, cancellationToken).ConfigureAwait(false);
                if (duplicated is not null)
                {
                    return duplicated;
                }

                throw;
            }

            return order;
        }

        // Milestone 2 - Step 3: validazione del dominio dell'ordine.
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
                throw new OrderValidationException($"Product {request.ProductCode} does not exist in the catalog.");
            }

            if (request.Quantity <= 0)
            {
                throw new OrderValidationException("Quantity must be greater than zero.");
            }
        }
    }

    // Milestone 2 - Step 2: repository concorrente con indice composto per la
    // deduplicazione.
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<Guid, OrderDto> _orders = new();
        private readonly ConcurrentDictionary<string, Guid> _index = new(StringComparer.OrdinalIgnoreCase);

        private static string ComposeKey(Guid userId, string productCode) => $"{userId:N}:{productCode}";

        public Task<OrderDto?> FindByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _orders.TryGetValue(orderId, out var order);
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
            var key = ComposeKey(order.UserId, order.ProductCode);

            if (!_orders.TryAdd(order.Id, order))
            {
                throw new OrderConflictException($"Order with id {order.Id} already exists.");
            }

            if (!_index.TryAdd(key, order.Id))
            {
                _orders.TryRemove(order.Id, out _);
                throw new OrderConflictException("An equivalent order already exists.");
            }

            return Task.CompletedTask;
        }
    }

    // Milestone 2 - Step 4: factory per costruire il servizio applicando DI manuale.
    public static class OrderServiceModule
    {
        public static IOrderWriteService CreateOrderWriteService(IOrderRepository? repository = null, IAppLogger? logger = null)
        {
            repository ??= new InMemoryOrderRepository();
            logger ??= new Microservices.Solution.UserService.ConsoleLogger("OrderService");
            return new OrderWriteService(repository, logger);
        }
    }
}
