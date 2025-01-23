namespace ECB.Domain.Models;

public class EcbCurrencyRatesSnapshot
{
    public string Date { get; set; } = null!;
    public List<CurrencyRate> Rates { get; set; } = null!;
}
