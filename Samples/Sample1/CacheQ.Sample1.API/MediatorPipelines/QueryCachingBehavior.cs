using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CacheQ.Sample1.API.MediatorPipelines
{
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
            if (_cachePolicy == null)
            {
                return await next();
            }
            if (_cacheManager.TryGetValue(
                _cachePolicy, 
                request,
                out TResult cachedResult))
            {
                return await Task.FromResult(cachedResult);
            }
            TResult result = await next();
            _cacheManager.SetItem(_cachePolicy, request, result);

            return result;
        }
    }
}
