namespace ECB.Domain.Models;

public class CurrencyRate
{
    public string Currency { get; set; } = null!;
    public decimal Rate { get; set; }
}