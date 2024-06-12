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
        private readonly IEntityService<TDomain> _service;
        private readonly IMapper _mapper;

        public GetByIdQueryHandler(IEntityService<TDomain> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task<TDto> Handle(GetByIdQuery<TDomain, TDto> request, CancellationToken cancellationToken)
        {
            var model = await _service.GetByIdAsync(request._id);
            var mapped = _mapper.Map<TDto>(model.Value);
            return mapped;
        }
    }
}
