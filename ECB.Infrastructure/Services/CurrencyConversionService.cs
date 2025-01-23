using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Interfaces.Services;
using ECB.Domain.Models;

namespace ECB.Infrastructure.Services;

public class CurrencyConversionService : ICurrencyConversionService
{
    private readonly ICurrencyRatesRepository _currencyRatesRepository;

    public CurrencyConversionService(ICurrencyRatesRepository currencyRatesRepository)
    {
        _currencyRatesRepository = currencyRatesRepository;
    }
    
    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        if (string.Equals(fromCurrency, toCurrency, StringComparison.OrdinalIgnoreCase))
        {
            return amount;
        }
        
        var rates = await _currencyRatesRepository.GetLatestRatesAsync();

        decimal fromRate = GetRateOrThrow(rates, fromCurrency);
        decimal toRate = GetRateOrThrow(rates, toCurrency);

        decimal convertedAmount = amount * (toRate / fromRate);

        return convertedAmount;
    }
    
    private decimal GetRateOrThrow(IReadOnlyList<CurrencyRate> rates, string currency)
    {
        if (rates == null || rates.Count == 0)
        {
            throw new InvalidOperationException("No currency rates available.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency must not be empty or whitespace.", nameof(currency));
        }

        currency = currency.Trim();

        var rateEntry = rates.FirstOrDefault(r =>
            r.Currency != null &&
            r.Currency.Trim().Equals(currency, StringComparison.OrdinalIgnoreCase));

        if (rateEntry == null)
        {
            throw new ArgumentException($"Currency '{currency}' not found in the latest rates.");
        }

        return rateEntry.Rate;
    }


}