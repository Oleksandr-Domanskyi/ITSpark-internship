using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Queries.GetById
{
    public class GetByIdQueryHandler<TDomain, TDto> : IRequestHandler<GetByIdQuery<TDomain, TDto>, TDto>
        where TDomain : Entity<Guid>
        where TDto : class
    {
        private readonly IEntityService<TDomain,TDto> _service;

        public GetByIdQueryHandler(IEntityService<TDomain,TDto> service)
        {
            _service = service;
        }
        public async Task<TDto> Handle(GetByIdQuery<TDomain, TDto> request, CancellationToken cancellationToken)
        {
            var model = await _service.GetByIdAsync(request._id);
            return model.Value;
        }
    }
}
