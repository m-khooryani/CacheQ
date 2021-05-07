using System;
using System.Collections.Concurrent;

namespace CacheQ
{
    internal class CacheManager : ICacheManager
    {
        private readonly ICacheStore _cacheStore;
        private readonly ICacheExpirationResolver _cacheExpirationResolver;

        public CacheManager(
            ICacheExpirationResolver cacheExpirationResolver,
            ICacheStore cacheStore)
        {
            _cacheExpirationResolver = cacheExpirationResolver;
            _cacheStore = cacheStore;
        }

        public bool TryGetValue<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            out TResult result)
        {
            if (!_cacheStore.ContainsKey(cachePolicy.Key(request)))
            {
                result = default;
                return false;
            }
            var t = _cacheStore.Get(cachePolicy.Key(request));

            if ((DateTime.UtcNow - t.DateTime) > 
                _cacheExpirationResolver.GetExpiryTime(cachePolicy.ExpirationLevel))
            {
                result = default;
                return false;
            }
            result = (TResult)t.Item;
            return true;
        }

        public void SetItem<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            TResult result)
        {
            _cacheStore.AddOrUpdate(
                cachePolicy.Key(request),
                new CacheValueModel(result));
        }
    }
}
