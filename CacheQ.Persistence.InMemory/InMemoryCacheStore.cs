using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace CacheQ.Persistence.InMemory
{
    internal class InMemoryCacheStore : ICacheStore
    {
        private readonly ConcurrentDictionary<string, CacheValueModel> _dictionary;
        private readonly ILogger<InMemoryCacheStore> _logger;

        public InMemoryCacheStore(ILogger<InMemoryCacheStore> logger)
        {
            _logger = logger;
            _dictionary = new ConcurrentDictionary<string, CacheValueModel>();
        }

        public void AddOrUpdate(string key, CacheValueModel cacheValueModel)
        {
            _logger.LogInformation("AddOrUpdating cache store");
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
            _logger.LogInformation($"Get item from store, Key: {key}");
            return _dictionary[key];
        }
    }
}
