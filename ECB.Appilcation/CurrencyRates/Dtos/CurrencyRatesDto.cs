using ECB.Domain.Models;

namespace ECB.Appilcation.CurrencyRates.Dtos;

public class CurrencyRatesDto
{
    public DateTime Date { get; set; }
    public List<CurrencyRate> Rates { get; set; } = null!;
}