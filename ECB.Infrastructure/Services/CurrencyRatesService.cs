using ECB.Domain.Interfaces.Services;
using ECB.Domain.Models;
using ECB.Infrastructure.Parsers;
using Microsoft.Extensions.Logging;
using ECB.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ECB.Infrastructure.Services;

public class CurrencyRatesService : ICurrencyRatesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CurrencyRatesService> _logger;
    private readonly ExternalApiOptions _options;

    public CurrencyRatesService(
        HttpClient httpClient,
        ILogger<CurrencyRatesService> logger,
        IOptions<ExternalApiOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
    }
    
    public async Task<EcbCurrencyRatesResponse> GetLatestRatesAsync()
    {
        _logger.LogInformation("Fetching latest ECB rates from {Url}", _options.EcbXmlUrl);
        
        try
        {
            var response = await _httpClient.GetAsync(_options.EcbXmlUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch rates. Status Code: {response.StatusCode}");
            }
            
            var content = await response.Content.ReadAsStringAsync();
            
            // Parse XML
            var ecbRates = EcbRatesParser.ParseEcbRatesFromXml(content);
            _logger.LogInformation("Successfully fetched and parsed ECB rates.");
            return ecbRates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching rates from ECB.");
            throw;
        }
    }

}