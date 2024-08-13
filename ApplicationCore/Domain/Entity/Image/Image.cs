using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entity.Image
{
    public class Image : Entity<Guid>
    {
        public string Path { get; set; } = default!;
        public Guid ProductId { get; set; }
    }
}
