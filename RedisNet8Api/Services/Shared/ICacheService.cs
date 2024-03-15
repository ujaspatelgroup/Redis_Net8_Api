namespace RedisNet8Api.Services.Shared
{
    public interface ICacheService
    {
        public Task<T> GetData<T>(string key);

        public Task<object> RemoveData(string key);

        public Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);
    }
}
