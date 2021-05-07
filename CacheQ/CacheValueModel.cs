using System;

namespace CacheQ
{
    public class CacheValueModel
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
