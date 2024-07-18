using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Dto
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public IList<string>? Role { get; set; }
    }
}