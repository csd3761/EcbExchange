using System.Data;
using System.Globalization;
using System.Text;
using ECB.Domain.Interfaces.Database;
using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ECB.Infrastructure.Repositories;

public class CurrencyRatesRepository : ICurrencyRatesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<CurrencyRatesRepository> _logger;

    public CurrencyRatesRepository(IDbConnectionFactory connectionFactory, ILogger<CurrencyRatesRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task UpsertCurrencyRatesAsync(EcbCurrencyRatesSnapshot ecbCurrencyRatesSnapshot)
    {
        var sql = new StringBuilder();
        sql.Append("INSERT INTO CurrencyRates (Currency, Rate, RateDate) VALUES ");

        for (int i = 0; i < ecbCurrencyRatesSnapshot.Rates.Count; i++)
        {
            var r = ecbCurrencyRatesSnapshot.Rates[i];
            sql.AppendFormat("('{0}', {1}, '{2}')", r.Currency, r.Rate.ToString(CultureInfo.InvariantCulture), ecbCurrencyRatesSnapshot.Date);
            if (i < ecbCurrencyRatesSnapshot.Rates.Count - 1) 
                sql.Append(", ");
        }

        sql.Append(" ON DUPLICATE KEY UPDATE Rate = VALUES(Rate);");
        
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();

            var rowsAffected = command.ExecuteNonQuery();
            _logger.LogInformation("Upserted {Count} rates. Rows affected: {RowsAffected}", ecbCurrencyRatesSnapshot.Rates.Count, rowsAffected);
        }
    }

    public async Task<CurrencyRate> GetCurrencyRateByNameAsync(string currency)
    {
        var sql = new StringBuilder();
        sql.Append("SELECT Currency, Rate FROM CurrencyRates WHERE CURRENCY = '");
        sql.Append(currency.Replace("'", "''"));
        sql.Append("' LIMIT 1;");

        using (var connection = await _connectionFactory.CreateConnectionAsync())
        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new CurrencyRate
                    {
                        Currency = reader.GetString(reader.GetOrdinal("Currency")),
                        Rate = reader.GetDecimal(reader.GetOrdinal("Rate"))
                    };
                }
            }
        }

        _logger.LogWarning("Currency rate not found for {Currency}", currency);
        return null;
    }

    public async Task<IReadOnlyList<CurrencyRate>> GetLatestRatesAsync()
    {
        var sql = new StringBuilder();
        sql.Append("SELECT Currency, Rate FROM CurrencyRates");

        var rates = new List<CurrencyRate>();
    
        using (var connection = await _connectionFactory.CreateConnectionAsync())
        using (var command = connection.CreateCommand())
        {
            command.CommandType = CommandType.Text;
            command.CommandText = sql.ToString();
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    rates.Add(new CurrencyRate
                    {
                        Currency = reader.GetString(reader.GetOrdinal("Currency")),
                        Rate = reader.GetDecimal(reader.GetOrdinal("Rate"))
                    });
                }
            }
        }

        if (rates.Count == 0)
        {
            _logger.LogWarning("No Currency rates found");
            return Array.Empty<CurrencyRate>();
        }

        return rates;
    }

}