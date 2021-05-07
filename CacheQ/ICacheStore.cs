namespace CacheQ
{
    public interface ICacheStore
    {
        bool ContainsKey(string key);
        CacheValueModel Get(string key);
        void AddOrUpdate(string key, CacheValueModel cacheValueModel);
    }
}
