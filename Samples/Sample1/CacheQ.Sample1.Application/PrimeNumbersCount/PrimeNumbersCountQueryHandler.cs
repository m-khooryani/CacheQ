using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CacheQ.Sample1.Application.PrimeNumbersCount;

class PrimeNumbersCountQueryHandler : IRequestHandler<PrimeNumbersCountQuery, int>
{
    public Task<int> Handle(PrimeNumbersCountQuery request, CancellationToken cancellationToken)
    {
        int count = 0;
        for (int i = request.StartRange; i < request.EndRange; i++)
        {
            if (IsPrimeSlow(i))
            {
                count++;
            }
        }
        Task.Delay(TimeSpan.FromSeconds(4), cancellationToken)
            .Wait(cancellationToken);
        return Task.FromResult(count);
    }

    private static bool IsPrimeSlow(int n)
    {
        int dividers = 0;
        for (int i = 1; i <= n; i++)
        {
            if (n % i == 0)
            {
                dividers++;
            }
        }
        return dividers == 2;
    }
}
