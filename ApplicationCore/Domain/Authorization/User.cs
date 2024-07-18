using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity;

namespace ApplicationCore.Domain.Authorization
{
    public class User
    {
        public string? Id { get; set; }
        public IList<string>? Role { get; set; } = default!;
        public string? Email { get; set; }
    }
}
