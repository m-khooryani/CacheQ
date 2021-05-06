using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CacheQ.Sample1.API.MediatorPipelines
{
    internal class QueryCachingBehavior<T, TResult> : IPipelineBehavior<T, TResult> where T : IRequest<TResult>
    {
        private readonly ICachePolicy<T> _cachePolicy;

        public QueryCachingBehavior(IEnumerable<ICachePolicy<T>> cachePolicy)
        {
            _cachePolicy = cachePolicy.SingleOrDefault();
        }

        public async Task<TResult> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            if (_cachePolicy == null)
            {
                return await next();
            }
            if (CacheManager<T, TResult>.TryGetValue(
                _cachePolicy, 
                request,
                out TResult cachedResult))
            {
                return await Task.FromResult(cachedResult);
            }
            TResult result = await next();
            CacheManager<T, TResult>.SetItem(_cachePolicy, request, result);

            return result;
        }
    }
}
