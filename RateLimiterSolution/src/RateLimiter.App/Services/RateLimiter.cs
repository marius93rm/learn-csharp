using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.App.Models;

namespace RateLimiter.App.Services;

/// <summary>
/// Implementazione completa del rate limiter asincrono utilizzato come soluzione di riferimento.
/// Controlla sia il numero massimo di richieste concorrenti tramite <see cref="SemaphoreSlim"/>
/// sia il numero di richieste accettate all'interno di una finestra temporale scorrevole.
/// </summary>
public sealed class RateLimiter
{
    private readonly RateLimiterOptions _options;
    private readonly SemaphoreSlim _semaphore;
    private readonly Queue<DateTime> _acceptedTimestamps = new();
    private readonly object _lock = new();

    /// <summary>
    /// Crea un nuovo rate limiter basato sulle opzioni fornite.
    /// </summary>
    /// <param name="options">Opzioni di configurazione del rate limiter.</param>
    public RateLimiter(RateLimiterOptions options)
    {
        _options = options;
        _semaphore = new SemaphoreSlim(options.MaxConcurrentRequests, options.MaxConcurrentRequests);
    }

    /// <summary>
    /// Prova a eseguire una richiesta simulata rispettando i limiti di concorrenza e di finestra temporale.
    /// </summary>
    /// <param name="request">Richiesta simulata da elaborare.</param>
    /// <param name="work">Delegato che rappresenta il lavoro da compiere se la richiesta viene accettata.</param>
    /// <param name="cancellationToken">Token opzionale per annullare l'operazione.</param>
    /// <returns>Un <see cref="RequestResult"/> che riporta l'esito dell'operazione.</returns>
    public async Task<RequestResult> ExecuteAsync(
        SimulatedRequest request,
        Func<CancellationToken, Task> work,
        CancellationToken cancellationToken = default)
    {
        var evaluationTime = DateTime.UtcNow;
        if (!TryEnterWindow(evaluationTime))
        {
            return new RequestResult
            {
                RequestId = request.Id,
                Accepted = false,
                StartTime = evaluationTime,
                EndTime = evaluationTime,
                Outcome = "Rifiutata per limite della finestra temporale"
            };
        }

        CancellationToken effectiveToken = cancellationToken;
        CancellationTokenSource? timeoutCts = null;
        try
        {
            if (_options.GlobalTimeout is TimeSpan timeout)
            {
                timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(timeout);
                effectiveToken = timeoutCts.Token;
            }

            await _semaphore.WaitAsync(effectiveToken).ConfigureAwait(false);

            var start = DateTime.UtcNow;
            try
            {
                await work(effectiveToken).ConfigureAwait(false);
                var end = DateTime.UtcNow;
                return new RequestResult
                {
                    RequestId = request.Id,
                    Accepted = true,
                    StartTime = start,
                    EndTime = end,
                    Outcome = "Completata"
                };
            }
            catch (OperationCanceledException)
            {
                var end = DateTime.UtcNow;
                return new RequestResult
                {
                    RequestId = request.Id,
                    Accepted = false,
                    StartTime = start,
                    EndTime = end,
                    Outcome = "Annullata"
                };
            }
            catch (Exception ex)
            {
                var end = DateTime.UtcNow;
                return new RequestResult
                {
                    RequestId = request.Id,
                    Accepted = false,
                    StartTime = start,
                    EndTime = end,
                    Outcome = $"Errore: {ex.Message}"
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }
        finally
        {
            timeoutCts?.Dispose();
        }
    }

    private bool TryEnterWindow(DateTime now)
    {
        lock (_lock)
        {
            var windowStart = now - _options.WindowLength;
            while (_acceptedTimestamps.Count > 0 && _acceptedTimestamps.Peek() < windowStart)
            {
                _acceptedTimestamps.Dequeue();
            }

            if (_acceptedTimestamps.Count >= _options.MaxRequestsPerWindow)
            {
                return false;
            }

            _acceptedTimestamps.Enqueue(now);
            return true;
        }
    }
}
