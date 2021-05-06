using System;

namespace CacheQ
{
    public class CacheExpirationSettings
    {
        public TimeSpan VeryShort { get; set; }
        public TimeSpan Short { get; set; }
        public TimeSpan Medium { get; set; }
        public TimeSpan Long { get; set; }
        public TimeSpan VeryLong { get; set; }
    }
}
