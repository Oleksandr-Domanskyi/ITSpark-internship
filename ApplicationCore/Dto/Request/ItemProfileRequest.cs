using ApplicationCore.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Dto.Request
{
    public class ItemProfileRequest
    {
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public string? Description { get; set; }
        public Category Category { get; set; }
        public string? CreatedBy {  get; set; }
        public List<IFormFile>? images { get; set; }
    }
}
