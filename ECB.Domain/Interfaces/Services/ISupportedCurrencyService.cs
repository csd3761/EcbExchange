namespace ECB.Domain.Interfaces.Services;

public interface ISupportedCurrencyService
{
    Task<bool> IsSupported(string currency);
}