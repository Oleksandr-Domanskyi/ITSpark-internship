using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;
using FluentResults;
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
        private readonly IUserContext _userContext;

        public DeleteCommandHandler(IEntityService<TDomain> service, IUserContext userContext)
        {
            _service = service;
            _userContext = userContext;
        }
        public async Task Handle(DeleteCommand<TDomain> request, CancellationToken cancellationToken)
        {

            if (await CheckAuthorization(request._id))
            {
                await _service.DeleteAsync(request._id);
            }
            else
            {
                throw new Exception("Access denied");
            }
        }

        private async Task<bool> CheckAuthorization(Guid id)
        {
            var model = (await _service.GetByIdAsync(id)).Value;
            var CurrentUser = new User()
            {
                Id = _userContext.GetCurrentUser()?.Id,
                Role = _userContext.GetCurrentUser()?.Roles!
            };
            if (CurrentUser.Id == null)
            {
                throw new Exception("Acces Denied");
            }
            var createdByUserId = typeof(TDomain).GetProperty("CreatedBy")?.GetValue(model)!;

            if (createdByUserId.ToString() == CurrentUser.Id || CurrentUser.Role == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
