using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Services;
using System.Security.Claims;
using ApplicationCore.Domain.Enum;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Dto.Response;
using AutoMapper;


namespace Applications.Services.UserService
{
    public interface ICheckUserService<TDomain, TDto>
    where TDomain : Entity<Guid>
    where TDto : class
    {
        public Task<bool> CheckUserAsync(Guid id);
        public bool CheckAdminAccess(FiltersOption option, out Filters filtersOption);
    }

    public class CheckUserService<TDomain, TDto> : ICheckUserService<TDomain, TDto>
    where TDomain : Entity<Guid>
    where TDto : class
    {
        private readonly IEntityService<TDomain, TDto> _service;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        private async Task<UserResponse> GetUser() => await _userContext.GetCurrentUser();

        public CheckUserService(IEntityService<TDomain, TDto> service,
         IUserContext userContext, IMapper mapper)
        {
            _service = service;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<bool> CheckUserAsync(Guid id)
        {
            var user = GetUser().Result;
            var model = (await _service.GetByIdAsync(id)).Value;
            var CurrentUser = new User()
            {
                Id = user.id,
                Role = user.Roles
            };
            if (CurrentUser.Id == null)
            {
                throw new Exception("Acces Denied");
            }
            var createdByUserId = typeof(TDto).GetProperty("CreatedBy")?.GetValue(model)!;

            return createdByUserId.ToString() == CurrentUser.Id || CurrentUser.Role!.Contains(UserRole.Admin.ToString());
        }
        public bool CheckAdminAccess(FiltersOption option, out Filters filtersOption)
        {            
            filtersOption = _mapper.Map<Filters>(option);
            var user = GetUser().Result;
            if (user.Roles!.Contains(UserRole.Admin.ToString()))
            {
                filtersOption.CreatedBy = user.id;
                filtersOption.Roles = UserRole.Admin.ToString();
                return true;
            }
            filtersOption.CreatedBy = user.id;
            filtersOption.Roles = UserRole.Customer.ToString();
            return false;
        }
    }


}