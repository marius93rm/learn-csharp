using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Shared;

namespace Microservices.OrderService
{
    /// <summary>
    /// Servizio responsabile della creazione ordini.
    /// Simula un endpoint POST /orders che riceve un DTO e delega la persistenza a un repository.
    /// </summary>
    public class OrderWriteService : IOrderWriteService
    {
        private readonly IOrderRepository _repository;

        public OrderWriteService(IOrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// TODO(8): Applicare validazioni di dominio (quantità positive, prodotto esistente, ecc.).
        /// TODO(9): Gestire idempotenza controllando se un ordine equivalente è già presente.
        /// </summary>
        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = new OrderDto(Guid.NewGuid(), request.UserId, request.ProductCode, request.Quantity);
            await _repository.AddAsync(order, cancellationToken).ConfigureAwait(false);
            return order;
        }
    }

    /// <summary>
    /// Repository in memoria che rispetta il principio di isolamento del servizio.
    /// Dimostra come mantenere la logica di accesso dati separata dalla logica di dominio.
    /// </summary>
    public class InMemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<Guid, OrderDto> _orders = new();

        public Task AddAsync(OrderDto order, CancellationToken cancellationToken)
        {
            // TODO(10): Introdurre controlli di concorrenza ottimistica (es. TryAdd) e gestire i conflitti.
            _orders[order.Id] = order;
            return Task.CompletedTask;
        }

        public Task<OrderDto?> FindByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            _orders.TryGetValue(orderId, out var order);
            return Task.FromResult(order);
        }
    }

    /// <summary>
    /// Modulo di composizione del microservizio ordini.
    /// Espone metodi helper per creare il servizio e semplificare i test.
    /// </summary>
    public static class OrderServiceModule
    {
        /// <summary>
        /// TODO(11): Estendere il modulo con factory per repository esterni (es. database relazionale).
        /// </summary>
        public static IOrderWriteService CreateOrderWriteService()
        {
            IOrderRepository repository = new InMemoryOrderRepository();
            return new OrderWriteService(repository);
        }
    }
}
