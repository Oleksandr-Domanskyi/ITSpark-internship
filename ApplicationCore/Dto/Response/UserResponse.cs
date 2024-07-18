using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Dto.Response
{
    public class UserResponse
    {
        public string? id { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
    }
}