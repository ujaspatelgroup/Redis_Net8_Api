using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using System.Data;
using System.Text.Json;

namespace RedisNet8Api.Services.Shared
{
    public class CacheService : ICacheService
    {

        private readonly string _RedisConnectionString;
        private IDatabase _cacheDb;
        private readonly IConfiguration _configuration;

        public CacheService(IConfiguration configuration)
        {
            _configuration = configuration;
            _RedisConnectionString = _configuration.GetValue<string>("RedisConnection:CacheConnection");
            var redis = ConnectionMultiplexer.Connect(_RedisConnectionString);
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T> GetData<T>(string key)
        {
            var redisValue = await _cacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(redisValue))
            {
                return JsonSerializer.Deserialize<T>(redisValue);
            }

            return default;
        }

        public async Task<object> RemoveData(string key)
        {
            var _exist = await _cacheDb.KeyExistsAsync(key);
            if (_exist)
            {
                return await _cacheDb.KeyDeleteAsync(key);
            }
            return false;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        }
    }
}
