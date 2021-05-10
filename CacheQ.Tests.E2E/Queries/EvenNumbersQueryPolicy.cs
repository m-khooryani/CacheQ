using System.Diagnostics.CodeAnalysis;

namespace CacheQ.Tests.E2E.Queries
{
    [ExcludeFromCodeCoverage]
    class EvenNumbersQueryPolicy : ICachePolicy<EvenNumbersQuery>
    {
        public CacheLevel ExpirationLevel => CacheLevel.Regular;

        public string Key(EvenNumbersQuery query)
        {
            return query.StartRange + "," + query.EndRange;
        }
    }
}
