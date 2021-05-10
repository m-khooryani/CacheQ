# CacheQ

CacheQ assists you to implement distributed cache simply! 
This is a simple sample usage with the CQRS pattern and MediatR:

first define a query


    public class EvenNumbersCountQuery : IRequest<int>
    {
        public int StartRange { get; set; }
        public int EndRange { get; set; }
    }

then cache policy

    class EvenNumbersCountQueryCachePolicy : ICachePolicy<EvenNumbersCountQuery>
    {
        public CacheLevel ExpirationLevel => CacheLevel.Regular;

        public string Key(EvenNumbersCountQuery query)
        {
            return query.StartRange + "," + query.EndRange;
        }
    }
    
MediatR caching Behaviour:

    internal class QueryCachingBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> 
        where TRequest : IRequest<TResult>
    {
        private readonly ICachePolicy<TRequest> _cachePolicy;
        private readonly ICacheManager _cacheManager;

        public QueryCachingBehavior(
            IEnumerable<ICachePolicy<TRequest>> cachePolicy,
            ICacheManager cacheManager)
        {
            _cachePolicy = cachePolicy.SingleOrDefault();
            _cacheManager = cacheManager;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            if (_cachePolicy is null)
            {
                return await next();
            }
            return await ReadOrUpdateCache(request, next);
        }

        private async Task<TResult> ReadOrUpdateCache(TRequest request, RequestHandlerDelegate<TResult> next)
        {
            if (_cacheManager.TryGetValue(
                            _cachePolicy,
                            request,
                            out TResult cachedResult))
            {
                return await Task.FromResult(cachedResult);
            }

            // Read From Handler
            TResult result = await next();

            // Update Cache
            _cacheManager.SetItem(_cachePolicy, request, result);

            return result;
        }
    }
