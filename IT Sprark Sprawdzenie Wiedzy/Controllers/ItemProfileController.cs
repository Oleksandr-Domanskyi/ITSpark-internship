using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.ItemProfile;
using ApplicationInfrastructure.Data;
using Applications.CQRS.Command.Create;
using Applications.CQRS.Command.Delete;
using Applications.CQRS.Command.Update;
using Applications.CQRS.Queries;
using Applications.CQRS.Queries.GetAll;
using Applications.CQRS.Queries.GetById;
using Applications.Dto;
using Applications.Dto.Request;
using Applications.Services;
using Ardalis.Specification.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using MvcRoute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace IT_Sprark_Sprawdzenie_Wiedzy.Controllers
{
    [MvcRoute("api/[controller]")]
    [ApiController]
    public class ItemProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFiltersService<ItemProfileDto> _filterService;
        private Filters<ItemProfileDto> AddFillters {  get; set; }

        public ItemProfileController(IMediator mediator, IFiltersService<ItemProfileDto>filterService)
        {
            _mediator = mediator;
            _filterService = filterService;
            AddFillters = new Filters<ItemProfileDto>();
        }

        [HttpGet("/ListOfItem")]
        public async Task<IActionResult> GetAll([FromQuery] FiltersOption model)
        {
            var item = await _mediator.Send(new GetAllQuery<ItemProfile, ItemProfileDto>());

            AddFillters.AddFilterOption(model, item);

            var filtered = await _filterService.AddFilters(AddFillters);
            return Ok(filtered);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var item = await _mediator.Send(new GetByIdQuery<ItemProfile, ItemProfileDto>(id));
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ItemProfileRequest request)
        {
            await _mediator.Send(new CreateCommand<ItemProfile, ItemProfileRequest>(request));
            return Created();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]ItemProfileRequest request)
        {
            await _mediator.Send(new UpdateCommand<ItemProfile, ItemProfileRequest>(request));
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]Guid id)
        {
            await _mediator.Send(new DeleteCommand<ItemProfile>(id));
            return NotFound();
        }
    }
}
