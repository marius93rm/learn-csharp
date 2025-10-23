namespace AsyncAwaitDashboard.App.Infrastructure;

public class NetworkLatencySimulator
{
    private readonly Random _random = new(2024);

    public async Task WaitAsync(string resourceName, CancellationToken cancellationToken = default)
    {
        // TODO [M1]: Simula una latenza di rete casuale (150-400 ms) usando Task.Delay e supportando la cancellazione.
        // Suggerimento: aggiungi un piccolo jitter differenziando per `resourceName` se vuoi rendere i tempi meno prevedibili.
        throw new NotImplementedException("TODO [M1]: implementa la simulazione di latenza asincrona.");
    }
}
