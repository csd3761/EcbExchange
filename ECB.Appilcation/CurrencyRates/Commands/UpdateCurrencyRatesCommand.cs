using ECB.Domain.Models;
using MediatR;

namespace ECB.Appilcation.CurrencyRates.Commands;

public class UpdateCurrencyRatesCommand : IRequest
{
    public EcbCurrencyRatesSnapshot EcbCurrencyRatesSnapshot { get; set; }

    public UpdateCurrencyRatesCommand(EcbCurrencyRatesSnapshot currencyRatesSnapshot)
    {
        EcbCurrencyRatesSnapshot = currencyRatesSnapshot;
    }
}