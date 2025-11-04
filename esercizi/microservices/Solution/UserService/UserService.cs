using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Solution.Shared;

namespace Microservices.Solution.UserService
{
    // Milestone 1 - Step 1: implementazione semplice di logger per isolare il servizio
    // dalle dipendenze infrastrutturali.
    public class ConsoleLogger : IAppLogger
    {
        private readonly string _scope;

        public ConsoleLogger(string scope)
        {
            _scope = scope;
        }

        public void Info(string message) => Write("INFO", message);

        public void Warning(string message) => Write("WARN", message);

        public void Error(string message, Exception? exception = null) => Write("ERROR", exception is null ? message : $"{message}: {exception.Message}");

        private static readonly object _sync = new();

        private void Write(string level, string message)
        {
            lock (_sync)
            {
                Console.WriteLine($"[{DateTimeOffset.UtcNow:O}] [{level}] [{_scope}] {message}");
            }
        }
    }

    public class UserValidationException : Exception
    {
        public UserValidationException(string message) : base(message)
        {
        }
    }

    // Milestone 1 - Step 3: servizio di lettura utenti con validazione dell'input
    // e logging esplicito.
    public class UserReadService : IUserReadService
    {
        private readonly IUserRepository _repository;
        private readonly IAppLogger _logger;

        public UserReadService(IUserRepository repository, IAppLogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
            {
                const string message = "UserId must be a non-empty GUID.";
                _logger.Warning(message);
                throw new UserValidationException(message);
            }

            _logger.Info($"Reading user {userId}.");
            var user = await _repository.FindByIdAsync(userId, cancellationToken).ConfigureAwait(false);

            if (user is null)
            {
                _logger.Warning($"User {userId} not found.");
            }

            return user;
        }
    }

    // Milestone 1 - Step 2: repository thread-safe con indice secondario per email.
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, UserDto> _users = new();
        private readonly ConcurrentDictionary<string, Guid> _indexByEmail = new(StringComparer.OrdinalIgnoreCase);

        public Task<UserDto?> FindByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _users.TryGetValue(userId, out var user);
            return Task.FromResult(user);
        }

        public Task<UserDto?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_indexByEmail.TryGetValue(email, out var id) && _users.TryGetValue(id, out var user))
            {
                return Task.FromResult<UserDto?>(user);
            }

            return Task.FromResult<UserDto?>(null);
        }

        public Task AddAsync(UserDto user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new UserValidationException("Email must be provided.");
            }

            if (_indexByEmail.ContainsKey(user.Email))
            {
                throw new UserValidationException($"User with email {user.Email} already exists.");
            }

            if (!_users.TryAdd(user.Id, user))
            {
                throw new UserValidationException($"A user with id {user.Id} already exists.");
            }

            if (!_indexByEmail.TryAdd(user.Email, user.Id))
            {
                _users.TryRemove(user.Id, out _);
                throw new UserValidationException($"User with email {user.Email} already exists.");
            }

            return Task.CompletedTask;
        }
    }

    // Milestone 1 - Step 4: factory che costruisce il servizio e popola dati di esempio
    // in modo idempotente.
    public static class UserServiceModule
    {
        public static IUserReadService CreateUserReadService(IUserRepository? repository = null, IAppLogger? logger = null)
        {
            repository ??= new InMemoryUserRepository();
            logger ??= new ConsoleLogger("UserService");
            SeedSampleData(repository, logger).GetAwaiter().GetResult();
            return new UserReadService(repository, logger);
        }

        private static readonly IReadOnlyList<UserDto> _seedUsers = new List<UserDto>
        {
            new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "demo@example.com"),
            new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "premium@example.com")
        };

        private static async Task SeedSampleData(IUserRepository repository, IAppLogger logger)
        {
            foreach (var user in _seedUsers)
            {
                try
                {
                    var existing = await repository.FindByIdAsync(user.Id, CancellationToken.None).ConfigureAwait(false);
                    if (existing is null)
                    {
                        await repository.AddAsync(user, CancellationToken.None).ConfigureAwait(false);
                        logger.Info($"Seeded user {user.Email}.");
                    }
                }
                catch (UserValidationException ex)
                {
                    logger.Warning($"Skipping seed for {user.Email}: {ex.Message}");
                }
            }
        }
    }
}
