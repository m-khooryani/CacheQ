using MediatR;

namespace CacheQ.Sample1.Application.PrimeNumbersCount
{
    public class PrimeNumbersCountQuery : IRequest<int>
    {
        public int StartRange { get; set; }
        public int EndRange { get; set; }
    }
}
