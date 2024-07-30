using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Authorization;
using ApplicationCore.Dto;
using ApplicationCore.Dto.Request;
using ApplicationCore.Dto.Response;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ApplicationInfrastructure.Contracts
{
    public interface IUserContext
    {
        public Task<bool> loginAsync(UserRequest model);
        public Task registerAsync(UserRequest model);
        public Task<UserResponse> GetCurrentUser();
        public Task ForgotPasswordAsync(string email);
        public Task ResetPasswordAsync(ResetPasswordRequest request);

    }
}