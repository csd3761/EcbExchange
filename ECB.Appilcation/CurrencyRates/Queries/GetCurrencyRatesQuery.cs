using ECB.Appilcation.CurrencyRates.Dtos;
using ECB.Domain.Models;
using MediatR;

namespace ECB.Appilcation.CurrencyRates.Queries;

public class GetCurrencyRatesQuery : IRequest<EcbCurrencyRatesSnapshot>
{
    
}