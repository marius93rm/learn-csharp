using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.App.Models;

namespace RateLimiter.App.Services;

/// <summary>
/// Genera richieste simulate e le invia al <see cref="RateLimiter"/>.
/// Questa versione fornisce l'implementazione completa utilizzata come possibile soluzione.
/// </summary>
public sealed class RequestSimulator
{
    private readonly RateLimiter _rateLimiter;
    private readonly RateLimiterOptions _options;

    /// <summary>
    /// Crea un nuovo simulatore di richieste.
    /// </summary>
    public RequestSimulator(RateLimiter rateLimiter, RateLimiterOptions options)
    {
        _rateLimiter = rateLimiter;
        _options = options;
    }

    /// <summary>
    /// Lancia in parallelo un certo numero di richieste simulate attraverso il rate limiter.
    /// </summary>
    /// <param name="totalRequests">Numero totale di richieste da generare.</param>
    /// <param name="cancellationToken">Token opzionale per annullare l'operazione.</param>
    /// <returns>Una collezione di <see cref="RequestResult"/> ordinata per identificatore.</returns>
    public async Task<IReadOnlyCollection<RequestResult>> RunBatchAsync(
        int totalRequests,
        CancellationToken cancellationToken = default)
    {
        if (totalRequests <= 0)
        {
            return Array.Empty<RequestResult>();
        }

        var requests = Enumerable.Range(1, totalRequests)
            .Select(id => new SimulatedRequest
            {
                Id = id,
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        var tasks = requests.Select(request => _rateLimiter.ExecuteAsync(
                request,
                async token => await Task.Delay(_options.SimulatedWorkDuration, token).ConfigureAwait(false),
                cancellationToken))
            .ToArray();

        try
        {
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results
                .OrderBy(result => result.RequestId)
                .ToArray();
        }
        catch (OperationCanceledException)
        {
            var canceledResults = tasks
                .Where(task => task.IsCompletedSuccessfully)
                .Select(task => task.Result)
                .ToList();

            return canceledResults
                .OrderBy(result => result.RequestId)
                .ToArray();
        }
    }
}
