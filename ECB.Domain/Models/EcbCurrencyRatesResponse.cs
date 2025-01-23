namespace ECB.Domain.Models;

public class EcbCurrencyRatesResponse
{
    public string Time { get; set; } = null!;
    public List<CurrencyRate> Rates { get; set; } = null!;
}