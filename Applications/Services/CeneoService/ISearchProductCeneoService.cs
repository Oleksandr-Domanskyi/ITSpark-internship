using ApplicationCore.Domain.Entity.Ceneo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.CeneoService
{
    public interface ISearchProductCeneoService
    {
        public CeneoProduct Search(string productName);
    }
}
