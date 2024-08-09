using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Product;
using ApplicationInfrastructure.Services.ImageService;
using Applications.Dto;

namespace ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFProductGenerator.PDFGenerationHelper
{
    public class ImageLoadingHelper
    {
        private readonly IImageAzureService<Product, ProductDto> _imageAzureService;

        public ImageLoadingHelper(IImageAzureService<Product, ProductDto> imageAzureService)
        {
            _imageAzureService = imageAzureService;
        }
        public async Task<(ProductDto Product, IEnumerable<Stream> Images)[]> FetchProductImagesAsync(IEnumerable<ProductDto> products)
        {
            var productTasks = products.Select(async product =>
            {
                var images = await _imageAzureService.LoadImagesAsStreamAsync(product.images);
                return (Product: product, Images: images);
            });

            return await Task.WhenAll(productTasks);
        }
    }
}