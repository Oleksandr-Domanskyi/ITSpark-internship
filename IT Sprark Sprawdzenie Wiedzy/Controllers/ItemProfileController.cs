using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.ItemProfile;
using ApplicationInfrastructure.Data;
using ApplicationInfrastructure.Services.ImageService;
using Applications.CQRS.Command.Create;
using Applications.CQRS.Command.Delete;
using Applications.CQRS.Command.Update;
using Applications.CQRS.Queries;
using Applications.CQRS.Queries.GetAll;
using Applications.CQRS.Queries.GetById;
using Applications.Dto;
using Applications.Dto.Request;
using Applications.Services.FilterService;
using Google.Apis.Translate.v2.Data;
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
        private readonly IFiltersService<ItemProfile> _filterService;
        //private Filters<ItemProfileDto> AddFillters {  get; set; }
        private readonly IImageAzureService<ItemProfile,ItemProfileRequest> _imageAzureService;

        public ItemProfileController(IMediator mediator, IFiltersService<ItemProfile>filterService,
        IImageAzureService<ItemProfile,ItemProfileRequest> imageAzureService)
        {
            _imageAzureService = imageAzureService;
            _mediator = mediator;
            _filterService = filterService;
            //AddFillters = new Filters<ItemProfileDto>();
        }

        [HttpGet("/ListOfItem")]
        public async Task<IActionResult> GetAll([FromQuery] FiltersOption model)
        {
            var item = await _mediator.Send(new GetAllQuery<ItemProfile, ItemProfileDto>(model));


            //AddFillters.AddFilterOption(model, item);

            //var filtered = await _filterService.AddFilters(AddFillters);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var item = await _mediator.Send(new GetByIdQuery<ItemProfile, ItemProfileDto>(id));
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ItemProfileRequest request)
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
            await _mediator.Send(new DeleteCommand<ItemProfile, ItemProfileDto>(id));
            return NotFound();
        }
    }
}
