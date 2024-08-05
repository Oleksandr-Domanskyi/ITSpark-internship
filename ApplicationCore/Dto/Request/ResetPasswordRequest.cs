using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.CustomValidations;

namespace ApplicationCore.Dto.Request
{
    public class ResetPasswordRequest
    {
        [Required]
        public string? userId { get; set; }

        [Required]
        public string? code { get; set; }

        [Required]
        [PasswordCustomValidation]
        public string newPassword { get; set; } = default!;
    }
}