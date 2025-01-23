using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Interfaces.Services;

namespace ECB.Infrastructure.Services;

public class SupportedCurrencyService : ISupportedCurrencyService
{
    private readonly ICurrencyRatesRepository _currencyRatesRepository;

    public SupportedCurrencyService(ICurrencyRatesRepository currencyRatesRepository)
    {
        _currencyRatesRepository = currencyRatesRepository;
    }

    public async Task<bool> IsSupported(string currency)
    {
        var currencyRate = await this._currencyRatesRepository.GetCurrencyRateByNameAsync(currency);
        
        return currencyRate != null;
    }
}