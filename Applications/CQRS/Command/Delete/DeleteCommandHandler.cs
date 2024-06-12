using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Delete
{
    public class DeleteCommandHandler<TDomain> : IRequestHandler<DeleteCommand<TDomain>>
        where TDomain : Entity<Guid>
    {
        private readonly IEntityService<TDomain> _service;

        public DeleteCommandHandler(IEntityService<TDomain> service)
        {
            _service = service;
        }
        public async Task Handle(DeleteCommand<TDomain> request, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(request._id);
        }
    }
}
