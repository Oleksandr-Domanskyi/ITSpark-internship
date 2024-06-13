using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
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
        private readonly IEntityService<TDomain> _service;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetAllQueryHandler(IEntityService<TDomain> service, IMapper mapper, IUserContext userContext)
        {
            _service = service;
            _mapper = mapper;
            _userContext = userContext;
        }
        public async Task<IEnumerable<TDto>> Handle(GetAllQuery<TDomain, TDto> request, CancellationToken cancellationToken)
        {
            var model = await _service.GetListAsync();

            var role = _userContext.GetCurrentUser().Roles;

            return _mapper.Map<IEnumerable<TDto>>(model.Value);
        }
    }
}
