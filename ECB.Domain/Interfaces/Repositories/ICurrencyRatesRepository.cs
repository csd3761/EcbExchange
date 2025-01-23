using ECB.Domain.Models;

namespace ECB.Domain.Interfaces.Repositories;

public interface ICurrencyRatesRepository
{
    Task UpsertCurrencyRatesAsync(EcbCurrencyRatesSnapshot ecbCurrencyRatesSnapshot);
    Task<CurrencyRate> GetCurrencyRateByNameAsync(string name);
    Task<IReadOnlyList<CurrencyRate>> GetLatestRatesAsync();
}