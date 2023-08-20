namespace CacheQ.Sample1.Application.PrimeNumbersCount;

class PrimeNumbersCountQueryCachePolicy : ICachePolicy<PrimeNumbersCountQuery>
{
    public CacheLevel ExpirationLevel => CacheLevel.Regular;

    public string Key(PrimeNumbersCountQuery query)
    {
        return query.StartRange + "," + query.EndRange;
    }
}
