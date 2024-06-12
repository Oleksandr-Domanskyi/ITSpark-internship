using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Update
{
    public class UpdateCommandHandler<TDomain, TReq> : IRequestHandler<UpdateCommand<TDomain, TReq>>
        where TDomain : Entity<Guid>
        where TReq : class
    {
        private readonly IEntityService<TDomain> _service;
        private readonly IMapper _mapper;

        public UpdateCommandHandler(IEntityService<TDomain> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public async Task Handle(UpdateCommand<TDomain, TReq> request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<TDomain>(request);
            await _service.UpdateAsync(model);
        }
    }
}
