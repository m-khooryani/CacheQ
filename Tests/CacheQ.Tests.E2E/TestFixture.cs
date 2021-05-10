using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CacheQ.Tests.E2E.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheQ.Tests.E2E
{
    [ExcludeFromCodeCoverage]
    public class TestFixture
    {
        internal ServiceProvider ServiceProvider { get; private set; }

        public TestFixture()
        {
            var serviceCollection = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            serviceCollection.AddSingleton(provider => configuration);
            serviceCollection.AddLogging();
            serviceCollection.AddMediatR(typeof(EvenNumbersQuery).Assembly);
            serviceCollection.AddCacheQ(typeof(EvenNumbersQuery).Assembly,
                options =>
                {
                    options.UseDistributedMemoryCache(x => x.Clock = new CustomClock());
                    options.UsePrefixKey(type =>
                    {
                        return type.Name;
                    });
                });
            serviceCollection.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        internal void PassLongTime()
        {
            // cache values are shorter than one day
            CustomClock.DateTimeOffset = CustomClock.DateTimeOffset.AddDays(1);
        }

        internal void Reset()
        {
            EvenNumbersQueryHandler.Calculated = 0;
            CustomClock.DateTimeOffset = CustomClock.DateTimeOffset.AddDays(1);
        }

        internal Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> request)
        {
            using var scope = ServiceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return mediator.Send(request);
        }
    }
}
