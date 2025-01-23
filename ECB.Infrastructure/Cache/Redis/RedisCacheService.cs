using ECB.Infrastructure.Cache.Interfaces;
using StackExchange.Redis;

namespace ECB.Infrastructure.Cache.Redis;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IDatabase database)
    {
        _database = database;
    }

    // this is a random commenttttt
    public Task<T?> GetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }
}