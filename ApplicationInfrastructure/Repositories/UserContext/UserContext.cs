using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Enum;
using ApplicationCore.Dto;
using ApplicationCore.Dto.Request;
using ApplicationCore.Dto.Response;
using ApplicationInfrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ApplicationInfrastructure.Repositories.UserContext
{
    public class UserContext : IUserContext
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserResponse> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId!);

            var response = new UserResponse
            {
                id = userId,
                Email = user?.Email,
                Roles = await _userManager.GetRolesAsync(user!),
            };
            return response;
        }
        public async Task<bool> loginAsync(UserRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task registerAsync(UserRequest model)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
            }
        }
    }
}
