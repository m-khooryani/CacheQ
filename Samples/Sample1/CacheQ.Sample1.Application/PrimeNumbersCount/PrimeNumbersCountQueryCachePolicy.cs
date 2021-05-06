using System;

namespace CacheQ.Sample1.Application.PrimeNumbersCount
{
    class PrimeNumbersCountQueryCachePolicy : ICachePolicy<PrimeNumbersCountQuery>
    {
        public TimeSpan Duration()
        {
            return TimeSpan.FromSeconds(10);
        }

        public string Key(PrimeNumbersCountQuery query)
        {
            return query.StartRange + "," + query.EndRange;
        }
    }
}
