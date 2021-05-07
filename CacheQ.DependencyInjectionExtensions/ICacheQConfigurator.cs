namespace Microsoft.Extensions.DependencyInjection
{
    public interface ICacheQConfigurator
    {
        IServiceCollection Services { get; }
    }
}