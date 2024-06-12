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
        public Guid Id { get; set; } = default(Guid);
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public string? Description { get; set; }
        public Category Category { get; set; }
        //public IFormFile? images { get; set; }
    }
}
