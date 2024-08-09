using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using Applications.Contracts;
using Applications.Services.UserService;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Command.Delete
{
    public class DeleteCommandHandler<TDomain, TDto> : IRequestHandler<DeleteCommand<TDomain, TDto>>
        where TDomain : Entity<Guid>
        where TDto : class
    {
        private readonly IEntityService<TDomain, TDto> _service;
        private readonly IUserAccessManagerService<TDomain, TDomain> _checkUserService;

        public DeleteCommandHandler(IEntityService<TDomain, TDto> service, IUserAccessManagerService<TDomain, TDomain> checkUserService)
        {
            _service = service;
            _checkUserService = checkUserService;
        }
        public async Task Handle(DeleteCommand<TDomain, TDto> request, CancellationToken cancellationToken)
        {

            if (await _checkUserService.CheckUserAccessAsync(request._id))
            {
                await _service.DeleteAsync(request._id);
            }
            else
            {
                throw new Exception("Access denied");
            }
        }
    }
}
