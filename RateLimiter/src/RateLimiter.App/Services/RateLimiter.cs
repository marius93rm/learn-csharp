using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.App.Models;

namespace RateLimiter.App.Services;

/// <summary>
/// Gestisce l'accesso concorrente alle richieste simulando un rate limiter asincrono.
/// Gli studenti estenderanno questo scheletro nelle milestone 2 e 3 per gestire sia
/// la concorrenza che il limite per finestra temporale.
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
        // TODO Milestone 3: verificare il limite per finestra temporale utilizzando una Queue<DateTime>.
        // 1. Rimuovere i timestamp più vecchi di WindowLength.
        // 2. Se il numero di richieste accettate nella finestra è >= MaxRequestsPerWindow, rifiutare la richiesta.
        //    Restituire un RequestResult con Accepted = false e Outcome descrittivo.

        // TODO Milestone 2: gestire il limite di concorrenza con SemaphoreSlim.
        // 1. Attendere _semaphore.WaitAsync(cancellationToken) prima di eseguire il lavoro.
        // 2. Garantire che _semaphore.Release() venga chiamato in un blocco finally.
        // 3. Registrare StartTime ed EndTime.

        // Il codice seguente fornisce un risultato placeholder per mantenere il progetto compilabile.
        // Gli studenti dovranno sostituire questa logica con una vera implementazione.
        try
        {
            await Task.Yield();
            return new RequestResult
            {
                RequestId = request.Id,
                Accepted = false,
                StartTime = request.CreatedAt,
                EndTime = request.CreatedAt,
                Outcome = "TODO: applicare la logica di rate limiting"
            };
        }
        catch (OperationCanceledException)
        {
            // TODO Milestone 4: gestire in modo puntuale l'annullamento e propagare un RequestResult adeguato.
            return new RequestResult
            {
                RequestId = request.Id,
                Accepted = false,
                StartTime = request.CreatedAt,
                EndTime = request.CreatedAt,
                Outcome = "Operazione annullata"
            };
        }
    }
}
