using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Create
{
    public class CreateCommandHandler<TDomain, TReq> : IRequestHandler<CreateCommand<TDomain, TReq>>
        where TDomain : Entity<Guid>
        where TReq : class
    {
        private readonly IEntityService<TDomain,TReq> _service;

        public CreateCommandHandler(IEntityService<TDomain, TReq> service)
        {
            _service = service;

        }

        public async Task Handle(CreateCommand<TDomain, TReq> request, CancellationToken cancellationToken)
        {
            await _service.AddEntityAsync(request.request);
        }

        
    }
}
