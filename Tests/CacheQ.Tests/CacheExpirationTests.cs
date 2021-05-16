using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace CacheQ.Tests
{
    [ExcludeFromCodeCoverage]
    public class CacheExpirationTests
    {
        [Fact]
        public void GetExpiryTimeCacheLevelLong_SetCacheExpirationSettingsLong_ReturnsSameValue()
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
        public void GetExpiryTimeCacheLevelRegular_SetCacheExpirationSettingsRegular_ReturnsSameValue()
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
        public void GetExpiryTimeCacheLevelShort_SetCacheExpirationSettingsShort_ReturnsSameValue()
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
        public void GetExpiryTimeCacheLevelVeryLong_SetCacheExpirationSettingsVeryLong_ReturnsSameValue()
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
        public void GetExpiryTimeCacheLevelVeryShort_SetCacheExpirationSettingsVeryShort_ReturnsSameValue()
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
        public void GetExpiryTimeZiro_SetCacheExpirationSettingsRegular_ReturnsSameValue()
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
