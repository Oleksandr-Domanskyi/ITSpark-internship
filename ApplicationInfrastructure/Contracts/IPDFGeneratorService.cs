using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Product;
using Applications.Dto;

namespace ApplicationInfrastructure.Contracts
{
    public interface IPDFProductGeneratorService
    {
        Task<byte[]> PDFGenerateAsync(IEnumerable<ProductDto> products);
    }
}