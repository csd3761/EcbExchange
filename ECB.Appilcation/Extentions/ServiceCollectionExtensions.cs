using ECB.Appilcation.CurrencyRates.Mappings;
using ECB.Appilcation.CurrencyRates.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ECB.Appilcation.Extentions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCurrencyRatesQueryHandler).Assembly));
        services.AddAutoMapper(typeof(CurrencyRatesMappingProfile));
    }
}