using ApplicationCore.Domain.Entity.Ceneo;
using Applications.Services.CeneoService;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Applications.CQRS.Queries.GetProductPriceFromCeneo
{
    public class GetPriceByCeneoQueryHandler : IRequestHandler<GetPriceByCeneoQuery, CeneoProduct>
    {
        private readonly ISearchProductCeneoService _service;

        public GetPriceByCeneoQueryHandler(ISearchProductCeneoService service)
        {
            _service = service;
        }

        public async Task<CeneoProduct> Handle(GetPriceByCeneoQuery request, CancellationToken cancellationToken)
        {
            var productList = _service.Search(request.ProductNameRequest);
            return productList;
        }
    }
}
