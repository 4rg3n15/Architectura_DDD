using Arquitectura_DDD.Core.Interfaces.InterfacesApplicacion;
using Microsoft.Extensions.Caching.Memory;

namespace Arquitectura_DDD.Infraestructure.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };
        }

        public Task<T> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return Task.FromResult(value);
            }
            return Task.FromResult(default(T));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = expiration.HasValue
                ? new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration }
                : _cacheOptions;

            _memoryCache.Set(key, value, options);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

    }
}
