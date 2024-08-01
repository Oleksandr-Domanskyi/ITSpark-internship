using ApplicationCore.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Image;

namespace ApplicationCore.Domain.Entity.Product
{
    public class Product : Entity<Guid>
    {
        public string Name { get; set; } = default!;
        public double Price { get; set; } = default!;
        public string? Description { get; set; }
        public string Category { get; set; } = default!;
        public List<Image.Image>? images { get; set; }

    }
}
