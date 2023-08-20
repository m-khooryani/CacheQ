using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CacheQ.Sample1.Application.EvenNumbersCount;

class EvenNumbersCountQueryHandler : IRequestHandler<EvenNumbersCountQuery, int>
{
    public Task<int> Handle(EvenNumbersCountQuery request, CancellationToken cancellationToken)
    {
        int count = 0;
        for (int i = request.StartRange; i < request.EndRange; i++)
        {
            if (IsEven(i))
            {
                count++;
            }
        }
        Task.Delay(TimeSpan.FromSeconds(4), cancellationToken)
            .Wait(cancellationToken);
        return Task.FromResult(count);
    }

    private static bool IsEven(int n)
    {
        return n % 2 == 0;
    }
}
