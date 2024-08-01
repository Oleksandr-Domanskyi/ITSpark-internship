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
        private readonly IEmailSender _emailSernder;

        public UserContext(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSernder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSernder = emailSernder;
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
        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.userId!);
            if (user == null)
            {

                throw new Exception("Failed to reset password: User not found!!!");
            }
            var result = await _userManager.ResetPasswordAsync(user, request.code!.Replace(" ","+"), request.newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to reset password: Problems with reset password!!!");
            }
        }
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"http://localhost:5169/Account/ResetPassword?userId={user.Id}&code={code}";
            var subject = "Reset Your Password";
            var htmlMessage = $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>";

            await _emailSernder.SendEmailAsync(user.Email!, subject, htmlMessage);
        }
    }
}
