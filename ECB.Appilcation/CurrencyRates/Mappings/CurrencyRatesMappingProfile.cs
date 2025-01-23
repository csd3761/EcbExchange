using AutoMapper;
using ECB.Domain.Models;

namespace ECB.Appilcation.CurrencyRates.Mappings;

public class CurrencyRatesMappingProfile : Profile
{
    public CurrencyRatesMappingProfile()
    {
        CreateMap<EcbCurrencyRatesResponse, EcbCurrencyRatesSnapshot>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Time))
            .ForMember(dest => dest.Rates, opt => opt.MapFrom(src => src.Rates));

        CreateMap<CurrencyRate, CurrencyRate>();
    }
}