using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace CacheQ
{
    internal class CacheManager : ICacheManager
    {
        private readonly IDistributedCache _cache;
        private readonly ICacheExpirationResolver _cacheExpirationResolver;
        private readonly PrefixKeyResolver _prefixKeyResolver;
        private readonly ILogger<CacheManager> _logger;

        public CacheManager(
            ICacheExpirationResolver cacheExpirationResolver,
            IDistributedCache cache,
            PrefixKeyResolver prefixKeyResolver,
            ILogger<CacheManager> logger)
        {
            _cacheExpirationResolver = cacheExpirationResolver;
            _cache = cache;
            _prefixKeyResolver = prefixKeyResolver;
            _logger = logger;
        }

        public bool TryGetValue<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            out TResult result)
        {
            _logger.LogInformation("Checking cache...");
            var cachedString = _cache.GetString(Key(cachePolicy, request));
            if (cachedString == null)
            {
                _logger.LogInformation("Key not found in cache store");
                result = default;
                return false;
            }

            var cacheValue = JsonSerializer.Deserialize<CacheValueModel<TResult>>(cachedString);

            result = cacheValue.Item;
            _logger.LogInformation("Item found in cache");
            return true;
        }

        public void SetItem<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            TResult result)
        {
            _cache.SetAsync(
                Key(cachePolicy, request),
                Encoding.UTF8.GetBytes(JsonSerializer
                    .Serialize(new CacheValueModel<TResult>(result))),
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpirationResolver
                        .GetExpiryTime(cachePolicy.ExpirationLevel),
                });
        }

        private string Key<TRequest>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request)
        {
            _logger.LogDebug("Get Key of request");
            return _prefixKeyResolver.Func.Invoke(request.GetType()) +
                cachePolicy.Key(request);
        }
    }
}
