namespace AsyncAwaitDashboard.App.Providers;

public interface IQuoteProvider
{
    Task<QuoteSummary> GetQuoteAsync(CancellationToken cancellationToken = default);
}

public sealed record QuoteSummary(string Symbol, decimal Price, decimal ChangePercent);
