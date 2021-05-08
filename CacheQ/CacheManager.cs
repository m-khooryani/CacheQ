using System;
using Microsoft.Extensions.Logging;

namespace CacheQ
{
    internal class CacheManager : ICacheManager
    {
        private readonly ICacheStore _cacheStore;
        private readonly ICacheExpirationResolver _cacheExpirationResolver;
        private readonly PrefixKeyResolver _prefixKeyResolver;
        private readonly ILogger<CacheManager> _logger;

        public CacheManager(
            ICacheExpirationResolver cacheExpirationResolver,
            ICacheStore cacheStore, 
            PrefixKeyResolver prefixKeyResolver,
            ILogger<CacheManager> logger)
        {
            _cacheExpirationResolver = cacheExpirationResolver;
            _cacheStore = cacheStore;
            _prefixKeyResolver = prefixKeyResolver;
            _logger = logger;
        }

        public bool TryGetValue<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            out TResult result)
        {
            _logger.LogInformation("Checking cache...");
            if (!_cacheStore.ContainsKey(Key(cachePolicy, request)))
            {
                _logger.LogInformation("Key not found in cache store");
                result = default;
                return false;
            }
            var t = _cacheStore.Get(Key(cachePolicy, request));

            if ((DateTime.UtcNow - t.DateTime) > 
                _cacheExpirationResolver.GetExpiryTime(cachePolicy.ExpirationLevel))
            {
                _logger.LogInformation("Cache expired!");
                result = default;
                return false;
            }
            result = (TResult)t.Item;
            _logger.LogInformation("Item found in cache");
            return true;
        }

        public void SetItem<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            TResult result)
        {
            _cacheStore.AddOrUpdate(
                Key(cachePolicy, request),
                new CacheValueModel(result));
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
