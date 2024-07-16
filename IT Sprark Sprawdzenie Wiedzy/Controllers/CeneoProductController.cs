using Applications.CQRS.Queries.GetProductPriceFromCeneo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IT_Sprark_Sprawdzenie_Wiedzy.Controllers
{
    [ApiController]
    public class CeneoProductController : Controller
    {
        private readonly IMediator _mediator;

        public CeneoProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("api/ceneo/Product")]
        public IActionResult GetPriceByCeneo([FromQuery]string productName)
        {
            var responce = _mediator.Send(new GetPriceByCeneoQuery(productName));
            return Ok(responce);
        }
    }
}
