using System;

namespace CacheQ
{
    internal class CacheValueModel
    {
        public DateTime DateTime { get; }
        public object Item { get; }

        public CacheValueModel(object item)
        {
            Item = item;
            DateTime = DateTime.UtcNow;
        }
    }
}
