using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ApplicationCore.CustomValidations
{
    public class PasswordCustomValidation : ValidationAttribute
    {
        private const int MinimumLength = 6;
        private const int MaximumLength = 20;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string password || string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult("Password is required.");
            }

            var errors = ValidatePassword(password);

            if (errors.Count > 0)
            {
                return new ValidationResult(string.Join(" ", errors));
            }

            return ValidationResult.Success;
        }

        private List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            if (password.Length < MinimumLength)
            {
                errors.Add($"Password must be at least {MinimumLength} characters long.");
            }
            else if (password.Length > MaximumLength)
            {
                errors.Add($"Password must be no more than {MaximumLength} characters long.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errors.Add("Password must contain at least one uppercase letter.");
            }
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errors.Add("Password must contain at least one lowercase letter.");
            }
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errors.Add("Password must contain at least one digit.");
            }
            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                errors.Add("Password must contain at least one special character.");
            }
            return errors;
        }
    }
}
