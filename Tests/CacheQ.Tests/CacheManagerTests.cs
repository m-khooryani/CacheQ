using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using Xunit;

namespace CacheQ.Tests
{
    [ExcludeFromCodeCoverage]
    public class CacheManagerTests
    {
        [Fact]
        public void CacheManagerTryGetValue_SetNullValue_ReturnsNullValue()
        {
            var distributedCache = Substitute.For<IDistributedCache>();
            distributedCache.Get(Arg.Any<string>()).Returns(x => null);

            var prefixKeyResolver = new PrefixKeyResolver()
            {
                Func = x =>
                {
                    return x.Name;
                }
            };

            var cacheManager = new CacheManagerBuilder()
                .SetDistributedCache(distributedCache)
                .SetPrefixKeyResolver(prefixKeyResolver)
                .Build();

            var cachePolicy = Substitute.For<ICachePolicy<SomeQuery>>();
            cachePolicy.Key(Arg.Any<SomeQuery>()).Returns("someKey");
            cacheManager.TryGetValue(cachePolicy, 
                new SomeQuery(), 
                out ResponseModel a);

            Assert.Null(a);
        }

        [Fact]
        public void CacheManagerTryGetValue_SetCacheValue_ReturnsSameValue()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var cacheValueModel = 5;
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cacheValueModel));
            var distributedCache = Substitute.For<IDistributedCache>();
            distributedCache.Get(Arg.Any<string>()).Returns(x => bytes);

            var prefixKeyResolver = new PrefixKeyResolver()
            {
                Func = x =>
                {
                    return x.Name;
                }
            };

            var cacheManager = new CacheManagerBuilder()
                .SetDistributedCache(distributedCache)
                .SetPrefixKeyResolver(prefixKeyResolver)
                .Build();

            var cachePolicy = Substitute.For<ICachePolicy<SomeQuery>>();
            cachePolicy.Key(Arg.Any<SomeQuery>()).Returns("someKey");
            cacheManager.TryGetValue(cachePolicy, 
                new SomeQuery(), 
                out int a);

            Assert.Equal(5, a);
        }
    }

    public class SomeQuery
    {

    }

    public class ResponseModel
    {

    }
}
