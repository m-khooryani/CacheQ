# CacheQ

CacheQ assists you to implement distributed cache simply! 
This is a simple sample usage with the CQRS pattern and MediatR:

first define a query


    public class EvenNumbersCountQuery : IRequest<int>
    {
        public int StartRange { get; set; }
        public int EndRange { get; set; }
    }
