using System;
using Microsoft.Extensions.Caching.SqlServer;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static ICacheQConfigurator UseDistributedSqlServerCache(
        this ICacheQConfigurator cacheQConfigurator,
        Action<SqlServerCacheOptions> setupAction)
    {
        cacheQConfigurator.Services
            .AddDistributedSqlServerCache(setupAction);

        return cacheQConfigurator;
    }
}
