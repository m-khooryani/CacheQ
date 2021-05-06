using System;
using System.Collections.Concurrent;

namespace CacheQ
{
    internal class CacheManager : ICacheManager
    {
        private readonly ConcurrentDictionary<string, CacheValueModel> _dictionary;
        private readonly ICacheExpirationResolver _cacheExpirationResolver;

        public CacheManager(ICacheExpirationResolver cacheExpirationResolver)
        {
            _cacheExpirationResolver = cacheExpirationResolver;
            _dictionary = new ConcurrentDictionary<string, CacheValueModel>();
        }

        public bool TryGetValue<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            out TResult result)
        {
            if (!_dictionary.ContainsKey(cachePolicy.Key(request)))
            {
                result = default;
                return false;
            }
            var t = _dictionary[cachePolicy.Key(request)];

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
            _dictionary.AddOrUpdate(
                cachePolicy.Key(request),
                new CacheValueModel(result));
        }
    }
}
