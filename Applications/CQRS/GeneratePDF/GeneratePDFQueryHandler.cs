using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Filters;
using ApplicationCore.Domain.Entity.Product;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Services;
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
        private readonly ICheckUserService<Product, ProductDto> _checkUserService;

        public GeneratePDFQueryHandler(IPDFProductGeneratorService generateProductPDFService,
                                       IEntityService<Product, ProductDto> entityService,
                                       ICheckUserService<Product, ProductDto> checkUserService)
        {
            _checkUserService = checkUserService;
            _generateProductPDFService = generateProductPDFService;
            _entityService = entityService;
        }

        public async Task<byte[]> Handle(GenerateProductListPDFQuery request, CancellationToken cancellationToken)
        {
            Result<IEnumerable<ProductDto>> model;
            if (_checkUserService.CheckAdminAccess(new FiltersOption(), out var updateFilters))
            {
                model = await _entityService.GetListAsync(updateFilters);
            }
            else
            {
                model = await _entityService.GetListAsync(updateFilters);
            }
            return await _generateProductPDFService.PDFGenerateAsync(model.Value);

        }
    }

}