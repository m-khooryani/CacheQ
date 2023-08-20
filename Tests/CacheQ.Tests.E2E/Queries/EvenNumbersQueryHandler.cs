using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace CacheQ.Tests.E2E.Queries;

[ExcludeFromCodeCoverage]
class EvenNumbersQueryHandler : IRequestHandler<EvenNumbersQuery, Dto>
{
    public static int Calculated = 0;

    public Task<Dto> Handle(EvenNumbersQuery request, CancellationToken cancellationToken)
    {
        Calculated++;
        int count = 0;
        for (int i = request.StartRange; i < request.EndRange; i++)
        {
            if (IsEven(i))
            {
                count++;
            }
        }
        return Task.FromResult(new Dto
        {
            Count = count,
        });
    }

    private static bool IsEven(int n)
    {
        return n % 2 == 0;
    }
}
