using ApplicationCore.Domain.Authorization;
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
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserContext _userContext;

        public ItemProfileController(IMediator mediator, IUserContext userContext)
        {
            _mediator = mediator;
            _userContext = userContext;
        }

        [HttpGet("/ListOfItem")]
        public async Task<IActionResult> GetAll([FromQuery] FiltersOption model)
        {
            var item = await _mediator.Send(new GetAllQuery<ItemProfile, ItemProfileDto>(model));
            return Ok(item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var item = await _mediator.Send(new GetByIdQuery<ItemProfile, ItemProfileDto>(id));
            return Ok(item);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm]ItemProfileRequest request)
        {
            var user = _userContext.GetCurrentUser();
            request.CreatedBy = user!.Id;
            await _mediator.Send(new CreateCommand<ItemProfile, ItemProfileRequest>(request));
            return Created();
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm]ItemProfileRequest request, Guid itemProfileId)
        {
            var user = _userContext.GetCurrentUser();
            request.CreatedBy = user!.Id;
            await _mediator.Send(new UpdateCommand<ItemProfile, ItemProfileRequest>(request, itemProfileId));
            return Ok();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery]Guid id)
        {
            await _mediator.Send(new DeleteCommand<ItemProfile, ItemProfileDto>(id));
            return NotFound();
        }
    }
}
