using AsyncAwaitDashboard.App.Infrastructure;

namespace AsyncAwaitDashboard.App.Providers;

/// <summary>
/// Restituisce un riassunto leggibile delle quotazioni ricevute dal client simulato.
/// </summary>
public class QuoteProvider : IQuoteProvider
{
    private readonly SimulatedApiClient _apiClient;

    public QuoteProvider(SimulatedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<QuoteSummary> GetQuoteAsync(CancellationToken cancellationToken = default)
    {
        // Otteniamo i dati grezzi e li trasformiamo nel record atteso dal dashboard.
        var payload = await _apiClient.GetQuoteAsync(cancellationToken).ConfigureAwait(false);

        return new QuoteSummary(
            payload.Symbol,
            decimal.Round(payload.Price, 2, MidpointRounding.AwayFromZero),
            payload.ChangePercent);
    }
}
