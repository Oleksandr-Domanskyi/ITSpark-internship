using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Services;

namespace Applications.Services.UserService
{
    public interface ICheckUserService<TDomain, TDto>
    where TDomain : Entity<Guid>
    where TDto : class
    {
        public Task<bool> CheckUserAsync(Guid id);
    }

    public class CheckUserService<TDomain, TDto> : ICheckUserService<TDomain, TDto>
    where TDomain : Entity<Guid>
    where TDto : class
    {
        private readonly IEntityService<TDomain, TDto> _service;
        private readonly IUserContext _userContext;

        public CheckUserService(IEntityService<TDomain, TDto> service, IUserContext userContext)
        {
            _service = service;
            _userContext = userContext;
        }
        public async Task<bool> CheckUserAsync(Guid id)
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

            return createdByUserId.ToString() == CurrentUser.Id || CurrentUser.Role == "Admin";
        }
    }


}