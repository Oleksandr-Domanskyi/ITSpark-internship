using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Enum;
using ApplicationInfrastructure.Services;
using Applications.Contracts;
using Applications.Services.UserService;
using AutoMapper;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Queries.GetAll
{
    public class GetAllQueryHandler<TDomain, TDto> : IRequestHandler<GetAllQuery<TDomain, TDto>, IEnumerable<TDto>>
        where TDomain : Entity<Guid>
        where TDto : class
    {
        private readonly IEntityService<TDomain, TDto> _service;
        private readonly IUserAccessManagerService<TDomain, TDto> _userAccessManagerService;

        public GetAllQueryHandler(IEntityService<TDomain, TDto> service, IUserAccessManagerService<TDomain, TDto> userAccessManagerService)
        {
            _service = service;
            _userAccessManagerService = userAccessManagerService;
        }
        public async Task<IEnumerable<TDto>> Handle(GetAllQuery<TDomain, TDto> request, CancellationToken cancellationToken)
        {
            var generatedFilters = await _userAccessManagerService.GenerateFiltersBasedOnUser(request.Filters);

            return (await _service.GetListAsync(generatedFilters)).Value;

        }
    }
}
