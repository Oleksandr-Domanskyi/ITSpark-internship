using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entity.Ceneo
{
    public class CeneoProductInformation
    {
        public string? Details { get; set; }
        public decimal? Price { get; set; }
        public decimal? Ocena { get; set; }
        public int? IlostOpinji { get; set; }
    }

}
