using ECB.Domain.Models;

namespace ECB.Domain.Interfaces.Services;

public interface ICurrencyRatesService
{
    Task<EcbCurrencyRatesResponse> GetLatestRatesAsync();
}