using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CacheQ.Tests
{
    [ExcludeFromCodeCoverage]
    internal class CacheManagerBuilder
    {
        private IDistributedCache _cache = Substitute.For<IDistributedCache>();
        private ICacheExpirationResolver _cacheExpirationResolver = Substitute.For<ICacheExpirationResolver>();
        private PrefixKeyResolver _prefixKeyResolver = Substitute.For<PrefixKeyResolver>();
        private ILogger<CacheManager> _logger = Substitute.For<ILogger<CacheManager>>();

        public CacheManager Build()
        {
            return new CacheManager(_cacheExpirationResolver,
                _cache,
                _prefixKeyResolver,
                _logger);
        }

        public CacheManagerBuilder SetPrefixKeyResolver(PrefixKeyResolver prefixKeyResolver)
        {
            _prefixKeyResolver = prefixKeyResolver;
            return this;
        }

        public CacheManagerBuilder SetDistributedCache(IDistributedCache cache)
        {
            _cache = cache;
            return this;
        }

        public CacheManagerBuilder SetCacheExpirationResolver(ICacheExpirationResolver cacheExpirationResolver)
        {
            _cacheExpirationResolver = cacheExpirationResolver;
            return this;
        }
    }
}
