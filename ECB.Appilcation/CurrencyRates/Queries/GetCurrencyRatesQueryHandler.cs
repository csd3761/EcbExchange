using AutoMapper;
using ECB.Domain.Interfaces.Services;
using ECB.Domain.Models;
using MediatR;

namespace ECB.Appilcation.CurrencyRates.Queries;

public class GetCurrencyRatesQueryHandler : IRequestHandler<GetCurrencyRatesQuery, EcbCurrencyRatesSnapshot>
{
    private readonly ICurrencyRatesService _currencyRatesService;
    private readonly IMapper _mapper;

    public GetCurrencyRatesQueryHandler(
    ICurrencyRatesService currencyRatesService,
    IMapper mapper)
    {
        _currencyRatesService = currencyRatesService;
        _mapper = mapper;
    }
    
    public async Task<EcbCurrencyRatesSnapshot> Handle(GetCurrencyRatesQuery request, CancellationToken cancellationToken)
    {
        var ecbRates = await _currencyRatesService.GetLatestRatesAsync();
        
        var ecbRatesDto = _mapper.Map<EcbCurrencyRatesSnapshot>(ecbRates);
        
        return ecbRatesDto;
    }
}