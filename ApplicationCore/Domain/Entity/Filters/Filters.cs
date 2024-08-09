using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Enum;
using ApplicationCore.Dto.Response;

namespace ApplicationCore.Domain.Entity.Filters
{
    public class Filters : FiltersOption
    {
        public string? UserRoleAccess { get; set; }
        public string? CreatedBy { get; set; }


        public void AddUserInformation(UserResponse user)
        {
            CreatedBy = user.id;
            UserRoleAccess = user.Roles != null && user.Roles.Contains(UserRole.Admin.ToString())
                ? UserRole.Admin.ToString()
                : UserRole.Customer.ToString();
        }
    };

}
