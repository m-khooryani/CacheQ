using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace CacheQ.Tests
{
    [ExcludeFromCodeCoverage]
    public class CacheExpirationTests
    {
        [Fact]
        public void Test1()
        {
            var expiration = TimeSpan.FromSeconds(30);
            var cacheExpirationSettings = new CacheExpirationSettings()
            {
                Long = expiration,
            };
            var cacheExpirationResolver = new CacheExpirationResolver(cacheExpirationSettings);

            Assert.Equal(expiration, cacheExpirationResolver.GetExpiryTime(CacheLevel.Long));
        }

        [Fact]
        public void Test2()
        {
            var expiration = TimeSpan.FromSeconds(30);
            var cacheExpirationSettings = new CacheExpirationSettings()
            {
                Regular = expiration,
            };
            var cacheExpirationResolver = new CacheExpirationResolver(cacheExpirationSettings);

            Assert.Equal(expiration, cacheExpirationResolver.GetExpiryTime(CacheLevel.Regular));
        }

        [Fact]
        public void Test3()
        {
            var expiration = TimeSpan.FromSeconds(30);
            var cacheExpirationSettings = new CacheExpirationSettings()
            {
                Short = expiration,
            };
            var cacheExpirationResolver = new CacheExpirationResolver(cacheExpirationSettings);

            Assert.Equal(expiration, cacheExpirationResolver.GetExpiryTime(CacheLevel.Short));
        }

        [Fact]
        public void Test4()
        {
            var expiration = TimeSpan.FromSeconds(30);
            var cacheExpirationSettings = new CacheExpirationSettings()
            {
                VeryLong = expiration,
            };
            var cacheExpirationResolver = new CacheExpirationResolver(cacheExpirationSettings);

            Assert.Equal(expiration, cacheExpirationResolver.GetExpiryTime(CacheLevel.VeryLong));
        }

        [Fact]
        public void Test5()
        {
            var expiration = TimeSpan.FromSeconds(30);
            var cacheExpirationSettings = new CacheExpirationSettings()
            {
                VeryShort = expiration,
            };
            var cacheExpirationResolver = new CacheExpirationResolver(cacheExpirationSettings);

            Assert.Equal(expiration, cacheExpirationResolver.GetExpiryTime(CacheLevel.VeryShort));
        }

        [Fact]
        public void Test6()
        {
            var expiration = TimeSpan.FromSeconds(30);
            var cacheExpirationSettings = new CacheExpirationSettings()
            {
                Regular = expiration,
            };
            var cacheExpirationResolver = new CacheExpirationResolver(cacheExpirationSettings);

            Assert.Equal(expiration, cacheExpirationResolver.GetExpiryTime(0));
        }
    }
}
