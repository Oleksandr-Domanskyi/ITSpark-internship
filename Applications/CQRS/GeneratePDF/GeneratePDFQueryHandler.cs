using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.Product;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Services;
using Applications.Contracts;
using Applications.Dto;
using Applications.Services.UserService;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Applications.CQRS.GeneratePDF
{
    public class GeneratePDFQueryHandler : IRequestHandler<GenerateProductListPDFQuery, byte[]>
    {
        private readonly IPDFProductGeneratorService _generateProductPDFService;
        private readonly IEntityService<Product, ProductDto> _entityService;
        private readonly IUserAccessManagerService<Product, ProductDto> _userAccessManagerService;

        public GeneratePDFQueryHandler(IPDFProductGeneratorService generateProductPDFService,
                                       IEntityService<Product, ProductDto> entityService,
                                       IUserAccessManagerService<Product, ProductDto> userAccessManagerService)
        {
            _userAccessManagerService = userAccessManagerService;
            _generateProductPDFService = generateProductPDFService;
            _entityService = entityService;
        }

        public async Task<byte[]> Handle(GenerateProductListPDFQuery request, CancellationToken cancellationToken)
        {
            var generatedFilters = await _userAccessManagerService.GenerateFiltersBasedOnUser(new FiltersOption());
            var model = (await _entityService.GetListAsync(generatedFilters)).Value;

            return await _generateProductPDFService.PDFProductGenerateAsync(model);

        }
    }

}