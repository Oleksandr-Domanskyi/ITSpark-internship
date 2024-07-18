using ApplicationCore.Domain.Entity.Ceneo;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.CQRS.Queries.GetProductPriceFromCeneo
{
    public class GetPriceByCeneoQuery: IRequest<CeneoProduct>
    {
        public string ProductNameRequest {  get; set; }

        public GetPriceByCeneoQuery(string productNameRequest)
        {
            ProductNameRequest = productNameRequest;
        }
    }
}
