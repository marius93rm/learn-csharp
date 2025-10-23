using AsyncAwaitDashboard.App.Infrastructure;

namespace AsyncAwaitDashboard.App.Providers;

public class QuoteProvider : IQuoteProvider
{
    private readonly SimulatedApiClient _apiClient;

    public QuoteProvider(SimulatedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<QuoteSummary> GetQuoteAsync(CancellationToken cancellationToken = default)
    {
        // TODO [M2]: Chiama `_apiClient.GetQuoteAsync` e crea il record QuoteSummary.
        // Suggerimento: arrotonda il prezzo a due decimali se necessario con `Math.Round` o `decimal.Round`.
        throw new NotImplementedException("TODO [M2]: implementa il recupero quotazioni.");
    }
}
