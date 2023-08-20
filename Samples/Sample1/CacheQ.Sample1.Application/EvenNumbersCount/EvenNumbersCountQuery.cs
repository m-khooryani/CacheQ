using MediatR;

namespace CacheQ.Sample1.Application.EvenNumbersCount;

public class EvenNumbersCountQuery : IRequest<int>
{
    public int StartRange { get; set; }
    public int EndRange { get; set; }
}
