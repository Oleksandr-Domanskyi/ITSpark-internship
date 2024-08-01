using ApplicationCore.Domain.Entity.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Applications.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public string? Description { get; set; }
        public string Category { get; set; } = default!;
        public List<Image>? images { get; set; }
    }
}
