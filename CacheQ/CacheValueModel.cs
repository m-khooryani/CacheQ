using System;

namespace CacheQ
{
    internal class CacheValueModel<T>
    {
        public DateTime DateTime { get; }
        public T Item { get; }

        public CacheValueModel(T item)
        {
            Item = item;
            DateTime = DateTime.UtcNow;
        }
    }
}
