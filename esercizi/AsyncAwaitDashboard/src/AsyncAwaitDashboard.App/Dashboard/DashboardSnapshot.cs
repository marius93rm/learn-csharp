using System.Text;
using AsyncAwaitDashboard.App.Providers;

namespace AsyncAwaitDashboard.App.Dashboard;

public sealed record DashboardSnapshot(
    WeatherSummary Weather,
    QuoteSummary Quote,
    SystemStatusSummary SystemStatus)
{
    public string ToConsoleString()
    {
        var builder = new StringBuilder()
            .AppendLine($"🌤️  Meteo: {Weather.Condition} | {Weather.TemperatureCelsius:F1} °C | Umidità {Weather.HumidityPercent}%")
            .AppendLine($"💹 Borsa: {Quote.Symbol} {Quote.Price:F2} ({Quote.ChangePercent:+0.##%;-0.##%;0%})")
            .AppendLine($"🛠️  Sistema: {(SystemStatus.IsDegraded ? "DEGRADED" : "OK")} | Job in coda: {SystemStatus.PendingJobs} | Ultimo backup {SystemStatus.LastBackupAge.TotalMinutes:F0} min fa");

        return builder.ToString();
    }
}
