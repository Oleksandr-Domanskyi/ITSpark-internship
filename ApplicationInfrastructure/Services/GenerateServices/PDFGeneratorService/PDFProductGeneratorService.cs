using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Image;
using ApplicationCore.Domain.Entity.Product;
using ApplicationInfrastructure.Contracts;
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

        private readonly IImageAzureService<Product, ProductDto> _imageAzureService;


        public PDFProductGeneratorService(IImageAzureService<Product, ProductDto> imageAzureService)
        {

            _imageAzureService = imageAzureService;
        }

        public async Task<byte[]> PDFGenerateAsync(IEnumerable<ProductDto> products)
        {

            var productImages = await LoadProductImagesAsync(products);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("List of Products")
                        .SemiBold().FontSize(20).FontColor(Colors.Black)
                        .AlignCenter();

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);
                            int index = 1;

                            foreach (var productImage in productImages)
                            {
                                var product = productImage.Product;
                                var images = productImage.Images;

                                column.Item().Column(col =>
                                {
                                    col.Item().Row(row =>
                                    {
                                        row.RelativeItem().Text($"{index}. {product.Name ?? "No Name"}")
                                            .SemiBold().FontSize(16).FontColor(Colors.Black);

                                        row.RelativeItem().AlignRight().Text($"${product.Price:F2}")
                                            .FontSize(14).FontColor(Colors.Black);
                                    });

                                    col.Item().Text($"Category: {product.Category ?? "No Category"}")
                                        .FontSize(14).FontColor(Colors.Black);

                                    col.Item().Text($"Description: {product.Description ?? "No Description"}")
                                        .FontSize(10).FontColor(Colors.Black);

                                    col.Item().Text("Images:")
                                        .FontSize(14).FontColor(Colors.Black);


                                    if (images.Any())
                                    {
                                        col.Item().Row(imageRow =>
                                        {
                                            foreach (var imageStream in images)
                                            {
                                                imageRow.RelativeItem()
                                                    .Image(imageStream)
                                                    .FitArea();
                                            }
                                        });
                                    }
                                    else
                                    {
                                        col.Item().Text("No Images Available")
                                            .FontSize(12).FontColor(Colors.Black);
                                    }
                                    column.Item().Element(container => container
                                                                        .Height(1)
                                                                        .Width(480)
                                                                        .Background(Colors.Black));
                                });
                                index++;
                            }
                        });
                });
            });
            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
        private async Task<(ProductDto Product, IEnumerable<Stream> Images)[]> LoadProductImagesAsync(IEnumerable<ProductDto> products)
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
