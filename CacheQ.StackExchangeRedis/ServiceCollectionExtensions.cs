using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ICacheQConfigurator UseStackExchangeRedisCache(
            this ICacheQConfigurator cacheQConfigurator,
            Action<RedisCacheOptions> setupAction)
        {
            cacheQConfigurator.Services
                .AddStackExchangeRedisCache(setupAction);

            return cacheQConfigurator;
        }
    }
}
