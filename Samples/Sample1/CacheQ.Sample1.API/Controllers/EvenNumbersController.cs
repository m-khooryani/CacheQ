using System.Threading.Tasks;
using CacheQ.Sample1.Application.EvenNumbersCount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CacheQ.Sample1.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EvenNumbersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EvenNumbersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<int> Get(
            int startRange = 1,
            int endRange = 1000)
        {
            return _mediator.Send(new EvenNumbersCountQuery()
            {
                StartRange = startRange,
                EndRange = endRange
            });
        }
    }
}
