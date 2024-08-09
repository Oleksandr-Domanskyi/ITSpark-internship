using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Product;
using ApplicationInfrastructure.Contracts;
using ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFFooterConfiguration;
using ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFHeaderConfiguration;
using ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFProductContentConfiguration;
using ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFProductGenerator.PDFGenerationHelper;
using ApplicationInfrastructure.Services.ImageService;
using Applications.Dto;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ApplicationInfrastructure.Services
{
    public class PDFProductGeneratorService : IPDFProductGeneratorService
    {
        static PDFProductGeneratorService() => QuestPDF.Settings.License = LicenseType.Community;
        private readonly ImageLoadingHelper _imageLoadingHelper;


        public PDFProductGeneratorService(IImageAzureService<Product, ProductDto> imageAzureService)
        {
            _imageLoadingHelper = new ImageLoadingHelper(imageAzureService);
        }
        public async Task<byte[]> PDFProductGenerateAsync(IEnumerable<ProductDto> products)
        {
            var productImages = await _imageLoadingHelper.FetchProductImagesAsync(products);
            var document = CreateDocument(productImages);

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        private Document CreateDocument((ProductDto Product, IEnumerable<Stream> Images)[] productImages)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    new PDFHeaderConfiguration().Apply(page);
                    new PDFProductContentConfiguration(productImages).Apply(page);
                    new PDFFooterConfiguration().Apply(page);
                });
            });
        }
    }
}
