namespace Microsoft.Extensions.DependencyInjection
{
    internal class CacheQConfigurator : ICacheQConfigurator
    {
        public IServiceCollection Services { get; }

        public CacheQConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}