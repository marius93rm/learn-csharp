using System;
using System.Threading;
using System.Threading.Tasks;
using Microservices.OrderService;
using Microservices.Shared;
using Microservices.UserService;

namespace Microservices.Gateway
{
    /// <summary>
    /// Il Gateway rappresenta il punto di accesso esterno: aggrega i microservizi
    /// e applica logiche trasversali (resilienza, logging, traduzione errori).
    /// </summary>
    public class OrderGateway : IOrderGateway
    {
        private readonly IUserReadService _userService;
        private readonly IOrderWriteService _orderService;
        private readonly IHttpClientSimulator _httpClient;

        /// <summary>
        /// Dependency Injection: ogni servizio è passato tramite interfaccia.
        /// TODO(12): Aggiungere un logger (es. interfaccia ILogger) per tracciare le richieste.
        /// </summary>
        public OrderGateway(IUserReadService userService, IOrderWriteService orderService, IHttpClientSimulator httpClient)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Flusso completo di orchestrazione:
        /// 1. Simula la ricezione della richiesta (ad es. HTTP POST /api/orders).
        /// 2. Richiede al UserService l'esistenza dell'utente.
        /// 3. Se l'utente esiste, crea l'ordine tramite OrderService.
        /// 4. Restituisce un DTO di conferma al chiamante.
        /// TODO(13): Implementare resilienza con timeout e retry per la chiamata al UserService.
        /// TODO(14): Gestire errori provenienti dall'OrderService e mapparli a messaggi comprensibili.
        /// </summary>
        public async Task<OrderConfirmationDto> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            // TODO(15): Validare la richiesta (es. verificare ProductCode non nullo) prima della chiamata ai servizi.
            await _httpClient.SimulateIncomingRequestAsync(cancellationToken).ConfigureAwait(false);

            var user = await _userService.GetUserAsync(request.UserId, cancellationToken).ConfigureAwait(false);
            if (user is null)
            {
                // TODO(16): Restituire errori strutturati (es. Result o eccezioni custom) invece di lanciare generiche.
                throw new InvalidOperationException($"User {request.UserId} not found");
            }

            var order = await _orderService.CreateOrderAsync(request, cancellationToken).ConfigureAwait(false);

            return new OrderConfirmationDto(order.Id, $"Order created for {user.Email}");
        }
    }

    /// <summary>
    /// Simula la componente infrastrutturale che riceve e invia chiamate HTTP.
    /// È un altro esempio di DIP: il Gateway dipende dall'interfaccia, non dalla classe concreta.
    /// </summary>
    public interface IHttpClientSimulator
    {
        Task SimulateIncomingRequestAsync(CancellationToken cancellationToken);
        Task SimulateOutgoingCallAsync(string endpointName, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Implementazione base che aggiunge un leggero delay per simulare la rete.
    /// </summary>
    public class HttpClientSimulator : IHttpClientSimulator
    {
        private readonly TimeSpan _latency;

        public HttpClientSimulator(TimeSpan? latency = null)
        {
            _latency = latency ?? TimeSpan.FromMilliseconds(50);
        }

        public async Task SimulateIncomingRequestAsync(CancellationToken cancellationToken)
        {
            // TODO(17): Aggiungere logiche di cancellazione e tracciamento (correlation id, ecc.).
            await Task.Delay(_latency, cancellationToken).ConfigureAwait(false);
        }

        public async Task SimulateOutgoingCallAsync(string endpointName, CancellationToken cancellationToken)
        {
            // TODO(18): Implementare metriche (es. registrare endpointName) e simulare errori/transient fault.
            await Task.Delay(_latency, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Modulo di composizione che collega i microservizi tra loro.
    /// Mostra come mantenere i servizi isolati e instanziati tramite interfacce.
    /// </summary>
    public static class GatewayModule
    {
        /// <summary>
        /// TODO(19): Permettere la configurazione esterna dei moduli (es. passare repository custom).
        /// </summary>
        public static IOrderGateway CreateOrderGateway()
        {
            var userService = UserServiceModule.CreateUserReadService();
            var orderService = OrderServiceModule.CreateOrderWriteService();
            IHttpClientSimulator httpClient = new HttpClientSimulator();

            return new OrderGateway(userService, orderService, httpClient);
        }
    }
}
