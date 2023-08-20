namespace CacheQ.Sample1.Application.EvenNumbersCount;

class EvenNumbersCountQueryCachePolicy : ICachePolicy<EvenNumbersCountQuery>
{
    public CacheLevel ExpirationLevel => CacheLevel.Regular;

    public string Key(EvenNumbersCountQuery query)
    {
        return query.StartRange + "," + query.EndRange;
    }
}
