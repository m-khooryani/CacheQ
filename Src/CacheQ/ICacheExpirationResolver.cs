using System;

namespace CacheQ
{
    public interface ICacheExpirationResolver
    {
        TimeSpan GetExpiryTime(CacheLevel cacheLevel);
    }
}
