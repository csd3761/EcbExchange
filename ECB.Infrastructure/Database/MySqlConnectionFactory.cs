using System.Data;
using ECB.Domain.Interfaces.Database;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace ECB.Infrastructure.Database;

public class MySqlConnectionFactory : IDbConnectionFactory
{
    private readonly string? _connectionString;

    public MySqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("EcbDbConnectionString");
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}