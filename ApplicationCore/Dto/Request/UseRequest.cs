using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Dto.Request
{
    public class UserRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}