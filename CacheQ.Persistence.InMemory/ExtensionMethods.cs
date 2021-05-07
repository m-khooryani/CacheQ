using System.Collections.Concurrent;

namespace CacheQ.Persistence.InMemory
{
    static class ExtensionMethods
    {
        public static void AddOrUpdate<K, V>(this ConcurrentDictionary<K, V> dictionary, K key, V value)
        {
            dictionary.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
        }
    }
}
