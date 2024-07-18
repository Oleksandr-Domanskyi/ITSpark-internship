using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationInfrastructure.Services;
using AutoMapper;
using MediatR;
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
        private readonly IEntityService<TDomain,TDto> _service;

        public GetAllQueryHandler(IEntityService<TDomain,TDto> service)
        {
            _service = service;
        }
        public async Task<IEnumerable<TDto>> Handle(GetAllQuery<TDomain, TDto> request, CancellationToken cancellationToken)
        {
            var model = (await _service.GetListAsync(request.Filters)).Value;
            return model;
        }
    }
}
