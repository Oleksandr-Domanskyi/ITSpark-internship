using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Authorization
{
    public interface IUserContext
    {
       public CurrentUser? GetCurrentUser();
    }


    public class UserContext : IUserContext
    {
       
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("Context user is not prezent");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return new CurrentUser(id: default, email: default, roles: "Anonymous");
            }
            var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return new CurrentUser(id, email, roles);
        }
       
    }
}
