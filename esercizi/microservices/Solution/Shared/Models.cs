using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Solution.Shared
{
    // Milestone 1 - Step 1: i DTO condivisi permettono ai microservizi di scambiarsi
    // contratti ben definiti senza dipendere dalle rispettive implementazioni.
    public record UserDto(Guid Id, string Email);

    // Milestone 2 - Step 1: il contratto ordini è definito qui così che gateway e
    // order service parlino la stessa lingua.
    public record OrderDto(Guid Id, Guid UserId, string ProductCode, int Quantity, DateTimeOffset CreatedAtUtc);

    public record CreateOrderRequest(Guid UserId, string ProductCode, int Quantity, string? RequestId = null);

    public record OrderConfirmationDto(Guid OrderId, string Message, DateTimeOffset CreatedAtUtc);

    public record OperationError(string Code, string Message);

    // Milestone 4 - Step 2: il gateway usa questa interfaccia minimale per applicare
    // logging strutturato e sostituire facilmente l'implementazione in fase di test.
    public interface IAppLogger
    {
        void Info(string message);
        void Warning(string message);
        void Error(string message, Exception? exception = null);
    }

    // Milestone 3 - Step 2 & Milestone 4 - Step 2: OperationResult permette di
    // propagare errori coerenti dal gateway al chiamante.
    public record OperationResult<T>(T? Value, OperationError? Error)
    {
        public bool IsSuccess => Error is null;

        public static OperationResult<T> Success(T value) => new(value, null);

        public static OperationResult<T> Failure(string code, string message) => new(default, new OperationError(code, message));
    }

    public interface IUserReadService
    {
        Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
    }

    // Milestone 1 - Step 1: interfaccia del repository utente con metodi necessari
    // per i controlli di duplicati e il seeding idempotente.
    public interface IUserRepository
    {
        Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserDto?> FindByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(UserDto user, CancellationToken cancellationToken);
    }

    public interface IOrderWriteService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    }

    // Milestone 2 - Step 2: il repository ordini espone operazioni dedicate per
    // verificare ordini duplicati e garantire idempotenza.
    public interface IOrderRepository
    {
        Task<OrderDto?> FindByIdAsync(Guid orderId, CancellationToken cancellationToken);
        Task<OrderDto?> FindByUserAndProductAsync(Guid userId, string productCode, CancellationToken cancellationToken);
        Task AddAsync(OrderDto order, CancellationToken cancellationToken);
    }

    public interface IOrderGateway
    {
        Task<OperationResult<OrderConfirmationDto>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    }
}
