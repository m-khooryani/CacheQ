using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CacheQ.Sample1.API.MediatorPipelines
{
    internal class QueryLoggingBehavior<T, TResult> : IPipelineBehavior<T, TResult> where T : IRequest<TResult>
    {
        private readonly ILogger<QueryLoggingBehavior<T, TResult>> _logger;

        public QueryLoggingBehavior(ILogger<QueryLoggingBehavior<T, TResult>> logger)
        {
            _logger = logger;
        }

        public async Task<TResult> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            _logger.LogInformation($"{request.GetType().Name} is processing: {Environment.NewLine}{JsonConvert.SerializeObject(request, Formatting.Indented)}");
            try
            {
                TResult result = await next();
                _logger.LogInformation($"Result: {Environment.NewLine}{JsonConvert.SerializeObject(result, Formatting.Indented)}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled Exception:{Environment.NewLine}{ex}");
                throw;
            }
            finally
            {
                _logger.LogDebug($"{request.GetType().Name} is processed.");
            }
        }
    }
}
