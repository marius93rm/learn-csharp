using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Shared;

namespace Microservices.UserService
{
    /// <summary>
    /// Rappresenta l'API pubblica del microservizio User.
    /// Si occupa solo di orchestrare la logica del dominio utente e di delegare
    /// l'accesso ai dati a un repository che rispetta il DIP.
    /// </summary>
    public class UserReadService : IUserReadService
    {
        private readonly IUserRepository _repository;

        /// <summary>
        /// Il servizio riceve il repository tramite Dependency Injection.
        /// In un'app reale lo risolveremmo da un container; qui lo passiamo a mano.
        /// </summary>
        public UserReadService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Simula l'endpoint GET /users/{id}.
        /// TODO(4): Aggiungere logiche di validazione (es. controllo formato email) e logging.
        /// </summary>
        public Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _repository.FindByIdAsync(userId, cancellationToken);
        }
    }

    /// <summary>
    /// Implementazione in memoria del repository utenti. Simula un database locale.
    /// Mostra come applicare il Repository Pattern separando il dominio dall'infrastruttura.
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, UserDto> _users = new();

        /// <summary>
        /// TODO(5): Valutare l'inserimento di regole di business (es. controllo duplicati) prima di salvare.
        /// </summary>
        public Task AddAsync(UserDto user, CancellationToken cancellationToken)
        {
            _users[user.Id] = user;
            return Task.CompletedTask;
        }

        public Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            _users.TryGetValue(userId, out var user);
            return Task.FromResult(user);
        }
    }

    /// <summary>
    /// Modulo di composizione per facilitare i test.
    /// Offre un esempio di semplice Dependency Injection manuale.
    /// </summary>
    public static class UserServiceModule
    {
        /// <summary>
        /// In un contesto reale potresti registrare queste dipendenze in un container.
        /// TODO(6): Estendere il modulo per supportare configurazioni alternative (es. repository su file).
        /// </summary>
        public static IUserReadService CreateUserReadService()
        {
            IUserRepository repository = new InMemoryUserRepository();
            SeedSampleData(repository).GetAwaiter().GetResult();
            return new UserReadService(repository);
        }

        private static async Task SeedSampleData(IUserRepository repository)
        {
            // TODO(7): Trasformare il seed in un meccanismo idempotente e gestire eventuali race condition.
            var demoUser = new UserDto(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "demo@example.com");
            await repository.AddAsync(demoUser, CancellationToken.None);
        }
    }
}
