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

namespace Applications.CQRS.Command.Create
{
    public class CreateCommandHandler<TDomain, TReq> : IRequestHandler<CreateCommand<TDomain, TReq>>
        where TDomain : Entity<Guid>
        where TReq : class
    {
        private readonly IEntityService<TDomain> _service;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateCommandHandler(IEntityService<TDomain> service,IMapper mapper, IUserContext userContext)
        {
            _service = service;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task Handle(CreateCommand<TDomain, TReq> request, CancellationToken cancellationToken)
        {
            
            await _service.AddEntityAsync(_mapper.Map<TDomain>(request));
        }
    }
}
