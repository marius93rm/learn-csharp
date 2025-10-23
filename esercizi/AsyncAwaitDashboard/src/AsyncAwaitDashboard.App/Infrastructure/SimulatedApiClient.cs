using System.Runtime.CompilerServices;
using AsyncAwaitDashboard.App.Providers;

namespace AsyncAwaitDashboard.App.Infrastructure;

public class SimulatedApiClient
{
    private readonly NetworkLatencySimulator _latency;
    private readonly Random _random = new(1337);

    private static readonly WeatherPayload[] WeatherSamples =
    {
        new("Torino", 21.5, 58, "Parzialmente nuvoloso", DateTime.UtcNow),
        new("Milano", 24.1, 65, "Sole pieno", DateTime.UtcNow),
        new("Bologna", 19.3, 70, "Pioggia leggera", DateTime.UtcNow),
        new("Roma", 27.8, 55, "Sole con nubi", DateTime.UtcNow)
    };

    private static readonly QuotePayload[] QuoteSamples =
    {
        new("ACME", 152.45m, 0.012m),
        new("CONTOSO", 89.13m, -0.004m),
        new("FABBRI", 42.00m, 0.006m)
    };

    private static readonly SystemStatusPayload[] StatusSamples =
    {
        new(3, TimeSpan.FromMinutes(18), false, DateTime.UtcNow),
        new(12, TimeSpan.FromMinutes(45), true, DateTime.UtcNow),
        new(0, TimeSpan.FromMinutes(5), false, DateTime.UtcNow)
    };

    private readonly DiagnosticEvent[] _diagnosticEvents =
    {
        new(DateTime.UtcNow.AddSeconds(-40), "INFO", "Worker-01 heartbeat ok"),
        new(DateTime.UtcNow.AddSeconds(-30), "INFO", "Worker-02 heartbeat ok"),
        new(DateTime.UtcNow.AddSeconds(-20), "WARN", "Worker-03 retry job #1247"),
        new(DateTime.UtcNow.AddSeconds(-10), "INFO", "Worker-04 heartbeat ok"),
        new(DateTime.UtcNow.AddSeconds(-5), "ERROR", "Worker-05 crashed (exit code 137)")
    };

    public SimulatedApiClient(NetworkLatencySimulator latency)
    {
        _latency = latency;
    }

    public async Task<WeatherPayload> GetWeatherAsync(CancellationToken cancellationToken = default)
    {
        await _latency.WaitAsync("weather", cancellationToken);
        var sample = WeatherSamples[_random.Next(WeatherSamples.Length)];
        return sample with { ObservedAt = DateTime.UtcNow };
    }

    public async Task<QuotePayload> GetQuoteAsync(CancellationToken cancellationToken = default)
    {
        await _latency.WaitAsync("quotes", cancellationToken);
        var sample = QuoteSamples[_random.Next(QuoteSamples.Length)];
        var drift = (decimal)(_random.NextDouble() * 0.02 - 0.01);
        return sample with { ChangePercent = decimal.Round(sample.ChangePercent + drift, 4) };
    }

    public async Task<SystemStatusPayload> GetSystemStatusAsync(CancellationToken cancellationToken = default)
    {
        await _latency.WaitAsync("system-status", cancellationToken);
        var sample = StatusSamples[_random.Next(StatusSamples.Length)];
        return sample with { CheckedAt = DateTime.UtcNow };
    }

    public async IAsyncEnumerable<DiagnosticEvent> StreamDiagnosticsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var evt in _diagnosticEvents)
        {
            await _latency.WaitAsync("diagnostics", cancellationToken);
            yield return evt with { Timestamp = DateTime.UtcNow };
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            await _latency.WaitAsync("diagnostics", cancellationToken);
            var severity = _random.NextDouble() switch
            {
                < 0.7 => "INFO",
                < 0.9 => "WARN",
                _ => "ERROR"
            };
            var message = severity switch
            {
                "INFO" => "Scheduled heartbeat",
                "WARN" => "Slow response on queue analytics",
                _ => "Database failover in progress"
            };
            yield return new DiagnosticEvent(DateTime.UtcNow, severity, message);
        }
    }

    public record WeatherPayload(string City, double TemperatureCelsius, int HumidityPercent, string Condition, DateTime ObservedAt);

    public record QuotePayload(string Symbol, decimal Price, decimal ChangePercent);

    public record SystemStatusPayload(int PendingJobs, TimeSpan LastBackupAge, bool IsDegraded, DateTime CheckedAt);
}
