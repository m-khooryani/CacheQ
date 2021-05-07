using System;

namespace CacheQ
{
    internal class CacheExpirationResolver : ICacheExpirationResolver
    {
        private readonly CacheExpirationSettings _expirationSettings;

        public CacheExpirationResolver(CacheExpirationSettings expirationSettings)
        {
            _expirationSettings = expirationSettings;
        }

        public TimeSpan GetExpiryTime(CacheLevel cacheLevel)
        {
            return cacheLevel switch
            {
                CacheLevel.VeryShort => _expirationSettings.VeryShort,
                CacheLevel.Short => _expirationSettings.Short,
                CacheLevel.Regular => _expirationSettings.Regular,
                CacheLevel.Long => _expirationSettings.Long,
                CacheLevel.VeryLong => _expirationSettings.VeryLong,
                _ => _expirationSettings.Regular,
            };
        }
    }
}
