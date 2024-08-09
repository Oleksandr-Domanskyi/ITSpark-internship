using System;
using System.Collections.Generic;
using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Services;
using ApplicationCore.Domain.Enum;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Dto.Response;
using AutoMapper;
using Applications.Contracts;


namespace Applications.Services.UserService
{

    public class UserAccessManagerService<TDomain, TDto> : IUserAccessManagerService<TDomain, TDto>
    where TDomain : Entity<Guid>
    where TDto : class
    {
        private readonly IEntityService<TDomain, TDto> _service;
        private readonly IUserContext _userContext;
        private readonly IFilterManagerService _filterManagerService;

        private async Task<UserResponse> GetUser() => await _userContext.GetCurrentUser();

        public UserAccessManagerService(IEntityService<TDomain, TDto> service,
         IUserContext userContext, IFilterManagerService filterManagerService)
        {
            _service = service;
            _userContext = userContext;
            _filterManagerService = filterManagerService;
        }

        public async Task<bool> CheckUserAccessAsync(Guid id)
        {
            var user = await GetUser();
            var model = (await _service.GetByIdAsync(id)).Value;

            if (user.id == null)
            {
                throw new Exception("Access Denied: User not Logged!!");
            }
            var createdByUserId = typeof(TDto).GetProperty("CreatedBy")?.GetValue(model)!;

            return createdByUserId.ToString() == user.id || user.Roles!.Contains(UserRole.Admin.ToString());
        }

        public async Task<Filters> GenerateFiltersBasedOnUser(FiltersOption option)
        {
            var user = await GetUser();
            var filters = _filterManagerService.GenerateFiltersBasedOnUserRole(option, user);

            return filters;
        }
    }


}