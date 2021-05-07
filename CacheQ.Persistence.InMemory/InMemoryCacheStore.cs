using System.Collections.Concurrent;

namespace CacheQ.Persistence.InMemory
{
    internal class InMemoryCacheStore : ICacheStore
    {
        private readonly ConcurrentDictionary<string, CacheValueModel> _dictionary;

        public InMemoryCacheStore()
        {
            _dictionary = new ConcurrentDictionary<string, CacheValueModel>();
        }

        public void AddOrUpdate(string key, CacheValueModel cacheValueModel)
        {
            _dictionary.AddOrUpdate(
                key,
                cacheValueModel);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public CacheValueModel Get(string key)
        {
            return _dictionary[key];
        }
    }
}
