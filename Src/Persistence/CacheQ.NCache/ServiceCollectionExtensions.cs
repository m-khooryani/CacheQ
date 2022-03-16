using Alachisoft.NCache.Caching.Distributed;
using Alachisoft.NCache.Caching.Distributed.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheQ.NCache;

public static class ServiceCollectionExtensions
{
    public static ICacheQConfigurator UseNCacheDistributedCache(
        this ICacheQConfigurator cacheQConfigurator,
        Action<NCacheConfiguration> configure)
    {
        cacheQConfigurator.Services.AddNCacheDistributedCache(configure);

        return cacheQConfigurator;
    }
}
