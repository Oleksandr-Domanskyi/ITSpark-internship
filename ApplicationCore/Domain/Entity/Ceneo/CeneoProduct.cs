using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entity.Ceneo
{
    public class CeneoProduct
    {
        public string? ProductName {  get; set; }
        public decimal? PriceCeneo { get; set; }

        public List<CeneoProductInformation>? productInformation { get; set; }
    }
}
