using System;

namespace AsyncAwaitDashboard.App.Infrastructure;

/// <summary>
/// Simula un'attesa di rete casuale propagando eventuali richieste di cancellazione.
/// </summary>
public class NetworkLatencySimulator
{
    private readonly Random _random = new(2024);

    public async Task WaitAsync(string resourceName, CancellationToken cancellationToken = default)
    {
        // Calcoliamo un ritardo casuale compreso fra 150 e 400 ms, aggiungendo un piccolo
        // offset in base alla risorsa per rendere i tempi più vari.
        var baseDelay = _random.Next(150, 401);
        var jitter = Math.Abs(HashCode.Combine(resourceName)) % 50;
        var delay = TimeSpan.FromMilliseconds(baseDelay + jitter);

        // Task.Delay supporta nativamente il CancellationToken, quindi la cancellazione
        // è immediata se richiesta dal chiamante.
        await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
    }
}
