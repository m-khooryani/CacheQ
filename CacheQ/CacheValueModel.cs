using System;

namespace CacheQ
{
    public class CacheValueModel<T>
    {
        public DateTime DateTime { get; }
        public T Item { get; }

        public CacheValueModel(
            T item,
            DateTime dateTime)
        {
            Item = item;
            DateTime = dateTime;
        }
    }
}
