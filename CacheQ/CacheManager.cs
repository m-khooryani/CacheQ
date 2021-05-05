using System;
using System.Collections.Concurrent;

namespace CacheQ
{
    public class CacheManager<TRequest, TResult>
    {
        private static readonly ConcurrentDictionary<string, Tuple<DateTime, TResult>> _dictionary;

        static CacheManager()
        {
            _dictionary = new ConcurrentDictionary<string, Tuple<DateTime, TResult>>();
        }

        public static TResult GetItem(ICachePolicy<TRequest> cachePolicy, TRequest request)
        {
            if (!_dictionary.ContainsKey(cachePolicy.Key(request)))
            {
                return default;
            }
            var t = _dictionary[cachePolicy.Key(request)];

            if ((DateTime.UtcNow - t.Item1) > cachePolicy.Duration())
            {
                return default;
            }

            return t.Item2;
        }

        public static void SetItem(
            ICachePolicy<TRequest> cachePolicy,
            TRequest request,
            TResult result)
        {
            _dictionary.AddOrUpdate(
                cachePolicy.Key(request), 
                new Tuple<DateTime, TResult>(DateTime.UtcNow, result));
        }
    }
}
