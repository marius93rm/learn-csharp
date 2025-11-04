using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Solution.OrderService;
using Microservices.Solution.Shared;
using Microservices.Solution.UserService;

namespace Microservices.Solution.Gateway
{
    // Milestone 3 - Step 2: il gateway coordina user service e order service.
    public class OrderGateway : IOrderGateway
    {
        private readonly IUserReadService _userService;
        private readonly IOrderWriteService _orderService;
        private readonly IHttpClientSimulator _httpClient;
        private readonly IAppLogger _logger;
        private readonly TimeSpan _userServiceTimeout = TimeSpan.FromMilliseconds(500);
        private readonly int _userServiceMaxAttempts = 3;

        public OrderGateway(IUserReadService userService, IOrderWriteService orderService, IHttpClientSimulator httpClient, IAppLogger logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<OperationResult<OrderConfirmationDto>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Milestone 3 - Step 2: validazione anticipata delle richieste.
                ValidateRequest(request);
            }
            catch (OrderValidationException ex)
            {
                return OperationResult<OrderConfirmationDto>.Failure("gateway.validation", ex.Message);
            }

            var correlationId = Guid.NewGuid().ToString("N");
            using var scope = new CorrelationScope(correlationId);
            _logger.Info($"[{correlationId}] Received request to create order for user {request.UserId}.");

            try
            {
                // Milestone 4 - Step 3: simuliamo latenza d'ingresso per misurare tempi end-to-end.
                await _httpClient.SimulateIncomingRequestAsync(correlationId, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return OperationResult<OrderConfirmationDto>.Failure("gateway.canceled", "Request was cancelled by the caller.");
            }

            UserDto user;
            try
            {
                // Milestone 4 - Step 1: retry con timeout nello user service.
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

            OrderDto order;
            try
            {
                // Milestone 3 - Step 2: chiamiamo l'order service solo se abbiamo un utente valido.
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

            // Milestone 4 - Step 3: simuliamo la chiamata uscente verso l'order service.
            await _httpClient.SimulateOutgoingCallAsync("OrderService", correlationId, cancellationToken).ConfigureAwait(false);

            var confirmation = new OrderConfirmationDto(order.Id, $"Order created for {user.Email}", order.CreatedAtUtc);
            _logger.Info($"[{correlationId}] Order {order.Id} created successfully for user {user.Email}.");
            return OperationResult<OrderConfirmationDto>.Success(confirmation);
        }

        private async Task<UserDto?> FetchUserWithRetryAsync(Guid userId, CancellationToken cancellationToken)
        {
            var attempt = 0;
            var exceptions = new List<Exception>();

            while (attempt < _userServiceMaxAttempts)
            {
                cancellationToken.ThrowIfCancellationRequested();
                attempt++;
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(_userServiceTimeout);

                try
                {
                    // Milestone 4 - Step 1: applichiamo un piccolo delay per simulare la chiamata HTTP.
                    await _httpClient.SimulateOutgoingCallAsync("UserService", CorrelationScope.CurrentId ?? string.Empty, timeoutCts.Token).ConfigureAwait(false);
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
                        throw;
                    }
                }
                catch (Exception ex) when (attempt < _userServiceMaxAttempts)
                {
                    exceptions.Add(ex);
                    _logger.Warning($"Transient error while calling UserService (attempt {attempt}): {ex.Message}");
                    await Task.Delay(TimeSpan.FromMilliseconds(100 * attempt), cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    throw;
                }
            }

            throw new TimeoutException("Unable to reach the user service after multiple attempts.", exceptions.Count > 0 ? exceptions[^1] : null);
        }

        private static void ValidateRequest(CreateOrderRequest request)
        {
            if (request is null)
            {
                throw new OrderValidationException("Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.ProductCode))
            {
                throw new OrderValidationException("ProductCode must be provided.");
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

    public interface IHttpClientSimulator
    {
        Task SimulateIncomingRequestAsync(string correlationId, CancellationToken cancellationToken);
        Task SimulateOutgoingCallAsync(string endpointName, string correlationId, CancellationToken cancellationToken);
    }

    // Milestone 4 - Step 3: simulatore HTTP per introdurre latenza e fault
    // controllati nelle chiamate tra servizi.
    public class HttpClientSimulator : IHttpClientSimulator
    {
        private readonly TimeSpan _latency;
        private readonly double _failureProbability;
        private readonly Random _random = new();

        public HttpClientSimulator(TimeSpan? latency = null, double failureProbability = 0.0)
        {
            _latency = latency ?? TimeSpan.FromMilliseconds(60);
            _failureProbability = Math.Clamp(failureProbability, 0, 1);
        }

        public async Task SimulateIncomingRequestAsync(string correlationId, CancellationToken cancellationToken)
        {
            await Task.Delay(_latency, cancellationToken).ConfigureAwait(false);
        }

        public async Task SimulateOutgoingCallAsync(string endpointName, string correlationId, CancellationToken cancellationToken)
        {
            await Task.Delay(_latency, cancellationToken).ConfigureAwait(false);

            if (_failureProbability > 0 && _random.NextDouble() < _failureProbability)
            {
                throw new InvalidOperationException($"Transient error contacting {endpointName}.");
            }
        }
    }

    // Milestone 4 - Step 3: scope asincrono per propagare il correlationId nei log.
    public sealed class CorrelationScope : IDisposable
    {
        private static readonly AsyncLocal<string?> _currentId = new();
        private readonly string? _previousId;

        public CorrelationScope(string correlationId)
        {
            _previousId = _currentId.Value;
            _currentId.Value = correlationId;
        }

        public static string? CurrentId => _currentId.Value;

        public void Dispose()
        {
            _currentId.Value = _previousId;
        }
    }

    // Milestone 3 - Step 1: factory che compone manualmente le dipendenze del gateway.
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
            httpClient ??= new HttpClientSimulator();
            logger ??= new ConsoleLogger("Gateway");
            return new OrderGateway(userService, orderService, httpClient, logger);
        }
    }
}
