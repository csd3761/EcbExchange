using System.Data;

namespace ECB.Domain.Interfaces.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}