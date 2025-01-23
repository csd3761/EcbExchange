using System.Data;
using System.Text;
using ECB.Appilcation.CurrencyRates.Queries;
using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace ECB.Infrastructure.BackgroundServices;

public class CurrencyRatesUpdaterHostedService : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CurrencyRatesUpdaterHostedService> _logger;
    private Timer? _timer;
    
    private readonly string _connectionString = "Server=localhost;User ID=root;Password=12345678;Database=EcbDb";

    public CurrencyRatesUpdaterHostedService(
        ILogger<CurrencyRatesUpdaterHostedService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        try
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var repository = scope.ServiceProvider.GetRequiredService<ICurrencyRatesRepository>();

                var rates = await mediator.Send(new GetCurrencyRatesQuery());

                if (rates == null)
                {
                    _logger.LogWarning("No rates returned from ECB.");
                    return;
                }

                // await UpsertCurrencyRates(rates);
                await repository.UpsertCurrencyRatesAsync(rates);
                
                _logger.LogInformation("ECB rates updated successfully.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during the periodic update of ECB rates.");
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}