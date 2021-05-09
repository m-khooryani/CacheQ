using System;

namespace CacheQ
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
