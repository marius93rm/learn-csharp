using AsyncAwaitDashboard.App.Infrastructure;

namespace AsyncAwaitDashboard.App.Providers;

public class WeatherProvider : IWeatherProvider
{
    private readonly SimulatedApiClient _apiClient;

    public WeatherProvider(SimulatedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<WeatherSummary> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        // TODO [M2]: Richiedi i dati meteo dal client asincrono e convertili in WeatherSummary usando await.
        // Suggerimento: considera `WeatherPayload.City` e `Condition` per generare descrizioni leggibili.
        throw new NotImplementedException("TODO [M2]: implementa il recupero meteo.");
    }
}
