using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entity.Filters
{
    public class Filters : FiltersOption
    {
        public string? Roles { get; set; }
        public string? CreatedBy { get; set; }
    }
}