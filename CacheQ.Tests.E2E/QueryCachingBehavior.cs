using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CacheQ.Tests.E2E
{
    [ExcludeFromCodeCoverage]
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
}
