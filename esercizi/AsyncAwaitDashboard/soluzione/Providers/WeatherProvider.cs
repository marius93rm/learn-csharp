using AsyncAwaitDashboard.App.Infrastructure;

namespace AsyncAwaitDashboard.App.Providers;

/// <summary>
/// Traduce il payload meteo in un record compatto utilizzato dal dashboard.
/// </summary>
public class WeatherProvider : IWeatherProvider
{
    private readonly SimulatedApiClient _apiClient;

    public WeatherProvider(SimulatedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<WeatherSummary> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        // Richiediamo i dati al client asincrono.
        var payload = await _apiClient.GetWeatherAsync(cancellationToken).ConfigureAwait(false);

        // Combiniamo città e condizione per restituire una descrizione parlante.
        var description = $"{payload.City}: {payload.Condition}";

        return new WeatherSummary(payload.TemperatureCelsius, description, payload.HumidityPercent);
    }
}
