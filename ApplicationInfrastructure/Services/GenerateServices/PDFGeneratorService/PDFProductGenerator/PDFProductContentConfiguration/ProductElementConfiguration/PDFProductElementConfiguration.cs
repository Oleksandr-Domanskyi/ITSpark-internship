using System.Collections.Generic;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Applications.Dto;
using QuestPDF.Helpers;

namespace ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFProductContentConfiguration.ProductElementConfiguration
{
    public class PDFProductElementConfiguration
    {
        private readonly (ProductDto Product, IEnumerable<Stream> Images) _productImage;
        private readonly int _index;

        public PDFProductElementConfiguration((ProductDto Product, IEnumerable<Stream> Images) productImage, int index)
        {
            _productImage = productImage;
            _index = index;
        }

        public void Apply(ColumnDescriptor column)
        {
            var product = _productImage.Product;
            var images = _productImage.Images;

            column.Item().Element(container =>
            {
                container.ShowEntire().Column(col =>
                {
                    CreateProductHeader(col, product, _index);
                    CreateProductInfo(col, product);
                    CreateProductDescription(col, product);
                    CreateProductImages(col, images);
                    CreateProductDivider(col);
                    col.Spacing(20);
                });
            });
        }

        private void CreateProductHeader(ColumnDescriptor col, ProductDto product, int index)
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text($"{index}. {product.Name ?? "No Name"}")
                    .SemiBold().FontSize(16).FontColor(Colors.Black);

                row.RelativeItem().AlignRight().Text($"${product.Price:F2}")
                    .FontSize(14).FontColor(Colors.Black);
            });
        }

        private void CreateProductInfo(ColumnDescriptor col, ProductDto product)
        {
            col.Item().Text($"Category: {product.Category ?? "No Category"}")
                .FontSize(14).FontColor(Colors.Black);
        }

        private void CreateProductDescription(ColumnDescriptor col, ProductDto product)
        {
            col.Item().Text($"Description: {product.Description ?? "No Description"}")
                .FontSize(10).FontColor(Colors.Black);
        }

        private void CreateProductImages(ColumnDescriptor col, IEnumerable<Stream> images)
        {
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
        }

        private void CreateProductDivider(ColumnDescriptor col)
        {
            col.Item().Element(divider => divider
                                              .Height(2)
                                              .Width(480)
                                              .Background(Colors.Black));
        }
    }
}
