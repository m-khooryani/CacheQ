using CacheQ;
using CacheQ.Persistence.InMemory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
	{
        public static ICacheQConfigurator UseInMemoryPersistence(
            this ICacheQConfigurator cacheQConfigurator)
        {
            cacheQConfigurator.Services
                .AddSingleton<ICacheStore, InMemoryCacheStore>();

            return cacheQConfigurator;
        }
    }
}
