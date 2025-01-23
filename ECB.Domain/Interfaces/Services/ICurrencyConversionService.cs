namespace ECB.Domain.Interfaces.Services;

public interface ICurrencyConversionService
{
    Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
}