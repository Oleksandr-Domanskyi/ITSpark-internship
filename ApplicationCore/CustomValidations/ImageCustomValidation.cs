using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.CustomValidations
{
    public class ImageCustomValidation : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public ImageCustomValidation(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not List<IFormFile> images)
            {
                return ValidationResult.Success!;
            }
            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"File '{image.FileName}' has an invalid extension. Allowed extensions are: {string.Join(", ", _allowedExtensions)}.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}