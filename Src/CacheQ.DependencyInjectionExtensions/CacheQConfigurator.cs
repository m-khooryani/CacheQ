using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
internal class CacheQConfigurator : ICacheQConfigurator
{
    public IServiceCollection Services { get; }

    public CacheQConfigurator(IServiceCollection services)
    {
        Services = services;
    }
}