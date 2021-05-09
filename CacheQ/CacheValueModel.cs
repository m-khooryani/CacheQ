using System;

namespace CacheQ
{
    public class CacheValueModel<T>
    {
        public DateTimeOffset DateTime { get; }
        public T Item { get; }

        public CacheValueModel(
            T item,
            DateTimeOffset dateTime)
        {
            Item = item;
            DateTime = dateTime;
        }
    }
}
