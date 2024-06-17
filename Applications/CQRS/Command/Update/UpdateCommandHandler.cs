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

namespace Applications.CQRS.Command.Update
{
    public class UpdateCommandHandler<TDomain, TReq> : IRequestHandler<UpdateCommand<TDomain, TReq>>
        where TDomain : Entity<Guid>
        where TReq : class
    {
        private readonly IEntityService<TDomain> _service;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public UpdateCommandHandler(IEntityService<TDomain> service, IMapper mapper, IUserContext userContext)
        {
            _service = service;
            _mapper = mapper;
            _userContext = userContext;
        }
        public async Task Handle(UpdateCommand<TDomain, TReq> request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<TDomain>(request);
            if(await CheckAuthorization((Guid)typeof(TDomain).GetProperty("Id")?.GetValue(model)!))
            {
                await _service.UpdateAsync(model);
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
