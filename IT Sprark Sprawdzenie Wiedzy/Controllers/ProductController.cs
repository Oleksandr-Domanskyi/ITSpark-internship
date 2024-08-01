using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.Product;
using Applications.CQRS.Command.Create;
using Applications.CQRS.Command.Delete;
using Applications.CQRS.Command.Update;
using Applications.CQRS.GeneratePDF;
using Applications.CQRS.Queries.GetAll;
using Applications.CQRS.Queries.GetById;
using Applications.CQRS.User.Queries.GetCurrentUser;
using Applications.Dto;
using Applications.Dto.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcRoute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace IT_Sprark_Sprawdzenie_Wiedzy.Controllers
{
    [MvcRoute("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("/GenerateListProductToPDF")]
        [Authorize]
        public async Task<IActionResult> GenerateListProductPDF()
        {
            byte[] fileContent = await _mediator.Send(new GenerateProductListPDFQuery());

            return File(fileContent, "application/pdf", "ProductList.pdf");
        }

        [HttpGet("/ListOfProduct")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] FiltersOption model)
        {
            var item = await _mediator.Send(new GetAllQuery<Product, ProductDto>(model));
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var item = await _mediator.Send(new GetByIdQuery<Product, ProductDto>(id));
            return Ok(item);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            var user = await _mediator.Send(new GetCurrentUserQuery());
            request.CreatedBy = user!.id;
            await _mediator.Send(new CreateCommand<Product, ProductRequest>(request));
            return Created();
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] ProductRequest request, Guid itemProfileId)
        {
            var user = await _mediator.Send(new GetCurrentUserQuery());
            request.CreatedBy = user!.id;
            await _mediator.Send(new
                UpdateCommand<Product, ProductDto, ProductRequest>(request, itemProfileId));
            return Ok();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            await _mediator.Send(new DeleteCommand<Product, ProductDto>(id));
            return NotFound();
        }

    }
}
