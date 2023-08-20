using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
	{
    public static ICacheQConfigurator UseDistributedMemoryCache(
        this ICacheQConfigurator cacheQConfigurator)
    {
        cacheQConfigurator.Services
            .AddDistributedMemoryCache();

        return cacheQConfigurator;
    }

    public static ICacheQConfigurator UseDistributedMemoryCache(
        this ICacheQConfigurator cacheQConfigurator,
        Action<MemoryDistributedCacheOptions> setupAction)
    {
        cacheQConfigurator.Services
            .AddDistributedMemoryCache(setupAction);

        return cacheQConfigurator;
    }
}
