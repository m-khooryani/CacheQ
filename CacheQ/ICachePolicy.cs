using System;

namespace CacheQ
{
    public interface ICachePolicy<T>
    {
        string Key(T query);
        TimeSpan Duration();
    }
}
