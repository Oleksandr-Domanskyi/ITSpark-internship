using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFProductContentConfiguration.ProductElementConfiguration;
using Applications.Dto;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFProductContentConfiguration
{
    public class PDFProductContentConfiguration
    {
        private readonly (ProductDto Product, IEnumerable<Stream> Images)[] _productImages;

        public PDFProductContentConfiguration((ProductDto Product, IEnumerable<Stream> Images)[] productImages)
        {
            _productImages = productImages;
        }

        public void Apply(PageDescriptor page)
        {
            page.Content()
                .PaddingVertical(1, Unit.Centimetre)
                .Column(column =>
                {
                    column.Spacing(20);
                    int index = 1;

                    foreach (var productImage in _productImages)
                    {
                        new PDFProductElementConfiguration(productImage, index).Apply(column);
                        index++;
                    }
                });
        }
    }
}