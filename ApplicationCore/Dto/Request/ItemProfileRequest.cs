using ApplicationCore.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Dto.Request
{
    public class ProductRequest
    {
        public string Name { get; set; } = default!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; } = default!;
        public string? Description { get; set; }
        public Category Category { get; set; }
        public string? CreatedBy { get; set; }
        public List<IFormFile>? images { get; set; }
    }
}
