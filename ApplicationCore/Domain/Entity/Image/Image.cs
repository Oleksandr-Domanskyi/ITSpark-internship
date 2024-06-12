using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entity.Image
{
    public class Image:Entity<Guid>
    {
        public string? ImageName { get; set; }
        public string? FileType { get; set; }
        public byte[]? DataFile { get; set; }
    }
}
