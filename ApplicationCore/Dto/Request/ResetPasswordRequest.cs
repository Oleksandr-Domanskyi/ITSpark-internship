using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Dto.Request
{
    public class ResetPasswordRequest
    {
        public string? userId { get; set; }
        public string? code { get; set; }
        public string newPassword { get; set; } = default!;
    }
}