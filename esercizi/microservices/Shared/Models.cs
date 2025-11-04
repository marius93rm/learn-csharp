using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Shared
{
    /// <summary>
    /// DTO condivisi tra i microservizi. Contengono solo dati, nessuna logica.
    /// Rispettano il principio di separazione tra contratti e implementazione.
    /// </summary>
    public record UserDto(Guid Id, string Email);

    public record OrderDto(Guid Id, Guid UserId, string ProductCode, int Quantity);

    public record CreateOrderRequest(Guid UserId, string ProductCode, int Quantity);

    public record OrderConfirmationDto(Guid OrderId, string Message);

    /// <summary>
    /// Contratto per leggere utenti. Esporta il concetto di REST GET /users/{id}.
    /// </summary>
    public interface IUserReadService
    {
        Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Contratto per salvare e leggere utenti, usato all'interno del microservizio.
    /// TODO(1): Estendere l'interfaccia con metodi aggiuntivi se servono ai requisiti.
    /// </summary>
    public interface IUserRepository
    {
        Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task AddAsync(UserDto user, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Contratto per la creazione ordini, equivalente a un endpoint POST /orders.
    /// </summary>
    public interface IOrderWriteService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Contratto interno per il repository ordini.
    /// TODO(2): Definire eventuali metodi di lettura necessari per garantire idempotenza.
    /// </summary>
    public interface IOrderRepository
    {
        Task<OrderDto?> FindByIdAsync(Guid orderId, CancellationToken cancellationToken);
        Task AddAsync(OrderDto order, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Contratto esposto dal Gateway per orchestrare la creazione degli ordini.
    /// TODO(3): Aggiungere metodi per altre operazioni (ad es. cancellazione ordine) se vuoi estendere l'esercizio.
    /// </summary>
    public interface IOrderGateway
    {
        Task<OrderConfirmationDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    }
}
