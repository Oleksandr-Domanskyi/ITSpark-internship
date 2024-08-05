using ApplicationCore.CustomValidations;
using ApplicationCore.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Applications.Dto.Request
{
    public class ProductRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name should have a minimum of 3 characters")]
        [MaxLength(30, ErrorMessage = "Name should have a maximum of 30 characters")]
        public string Name { get; set; } = default!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero and less than or equal to the maximum value")]
        public double Price { get; set; } = default!;

        [MaxLength(120, ErrorMessage = "Description should have a maximum of 120 characters")]
        public string? Description { get; set; }

        [Required]
        public Category Category { get; set; }

        [ImageCustomValidation(new[] { ".png", ".jpeg", ".jpg" })]
        public List<IFormFile>? Images { get; set; }

        [JsonIgnore]
        public string? CreatedBy { get; set; }
    }
}
