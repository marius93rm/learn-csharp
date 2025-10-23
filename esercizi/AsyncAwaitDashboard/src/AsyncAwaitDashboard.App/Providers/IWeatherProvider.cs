namespace AsyncAwaitDashboard.App.Providers;

public interface IWeatherProvider
{
    Task<WeatherSummary> GetCurrentAsync(CancellationToken cancellationToken = default);
}

public sealed record WeatherSummary(double TemperatureCelsius, string Condition, int HumidityPercent);
