using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.App.Models;

namespace RateLimiter.App.Services;

/// <summary>
/// Genera richieste simulate e le invia al <see cref="RateLimiter"/>.
/// Le milestone guidano lo studente a costruire batch concorrenti con Task e await.
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
    public Task<IReadOnlyCollection<RequestResult>> RunBatchAsync(
        int totalRequests,
        CancellationToken cancellationToken = default)
    {
        // TODO Milestone 1: generare una lista di SimulatedRequest con Id incrementale e CreatedAt = DateTime.UtcNow.
        // Suggerimento: Enumerable.Range può essere un buon punto di partenza.

        // TODO Milestone 1: per ogni richiesta, creare un Task che invochi RateLimiter.ExecuteAsync
        // passando una funzione di lavoro che esegua Task.Delay(_options.SimulatedWorkDuration, cancellationToken).

        // TODO Milestone 1: attendere il completamento di tutti i task con Task.WhenAll e restituire i risultati.
        // Non dimenticare di ordinare i risultati per Id per una visualizzazione coerente.

        return Task.FromResult<IReadOnlyCollection<RequestResult>>(Array.Empty<RequestResult>());
    }
}
