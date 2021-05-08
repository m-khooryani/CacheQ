using System;

namespace CacheQ
{
    internal class CacheManager : ICacheManager
    {
        private readonly ICacheStore _cacheStore;
        private readonly ICacheExpirationResolver _cacheExpirationResolver;
        private readonly PrefixKeyResolver _prefixKeyResolver;

        public CacheManager(
            ICacheExpirationResolver cacheExpirationResolver,
            ICacheStore cacheStore, 
            PrefixKeyResolver prefixKeyResolver)
        {
            _cacheExpirationResolver = cacheExpirationResolver;
            _cacheStore = cacheStore;
            _prefixKeyResolver = prefixKeyResolver;
        }

        public bool TryGetValue<TRequest, TResult>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            out TResult result)
        {
            if (!_cacheStore.ContainsKey(Key(cachePolicy, request)))
            {
                result = default;
                return false;
            }
            var t = _cacheStore.Get(Key(cachePolicy, request));

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
                Key(cachePolicy, request),
                new CacheValueModel(result));
        }

        private string Key<TRequest>(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request)
        {
            return _prefixKeyResolver.Func.Invoke(request.GetType()) +
                cachePolicy.Key(request);
        }
    }
}
