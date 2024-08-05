using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.CustomValidations;

namespace ApplicationCore.Dto.Request
{
    public class UserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
        [Required]
        [PasswordCustomValidation]
        public string Password { get; set; } = default!;
    }
}