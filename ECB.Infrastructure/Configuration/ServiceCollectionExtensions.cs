using ECB.Domain.Interfaces.Database;
using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Interfaces.Services;
using ECB.Domain.Interfaces.Strategies;
using ECB.Infrastructure.BackgroundServices;
using ECB.Infrastructure.Database;
using ECB.Infrastructure.Persistence;
using ECB.Infrastructure.Repositories;
using ECB.Infrastructure.Services;
using ECB.Infrastructure.Strategies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECB.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        
        services.AddHostedService<CurrencyRatesUpdaterHostedService>();
        
        services.AddScoped<ICurrencyRatesRepository, CurrencyRatesRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ISupportedCurrencyService, SupportedCurrencyService>();
        services.AddScoped<ICurrencyConversionService, CurrencyConversionService>();
        
        services.AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();
        services.AddSingleton<IWalletBalanceStrategy, AddFundsStrategy>();
        services.AddSingleton<IWalletBalanceStrategy, SubtractFundsStrategy>();
        services.AddSingleton<IWalletBalanceStrategy, ForceSubtractFundsStrategy>();
        services.AddSingleton<IWalletBalanceStrategyFactory, WalletBalanceStrategyFactory>();

        services.AddDbContext<EcbDbContext>(options =>
        {
            options.UseMySql(
                configuration.GetConnectionString("EcbDbConnectionString"), 
                ServerVersion.AutoDetect(configuration.GetConnectionString("EcbDbConnectionString"))
                );
        });
    }
}