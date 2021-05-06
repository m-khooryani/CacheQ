using System;
using System.Collections.Concurrent;

namespace CacheQ
{
    public class CacheManager<TRequest, TResult>
    {
        private static readonly ConcurrentDictionary<string, CacheValueModel<TResult>> _dictionary;

        static CacheManager()
        {
            _dictionary = new ConcurrentDictionary<string, CacheValueModel<TResult>>();
        }

        public static bool TryGetValue(
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

            if ((DateTime.UtcNow - t.DateTime) > cachePolicy.Duration())
            {
                result = default;
                return false;
            }
            result = t.Item;
            return true;
        }

        public static void SetItem(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            TResult result)
        {
            _dictionary.AddOrUpdate(
                cachePolicy.Key(request),
                new CacheValueModel<TResult>(result));
        }
    }
}
