using CacheQ;
using CacheQ.Persistence.InMemory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInMemoryPersistence(
			   this IServiceCollection services)
		{
			services.AddSingleton<ICacheStore, InMemoryCacheStore>();

			return services;
		}
	}
}
