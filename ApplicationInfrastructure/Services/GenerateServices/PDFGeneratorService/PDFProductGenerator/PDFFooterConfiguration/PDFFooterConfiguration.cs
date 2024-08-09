using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;

namespace ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFFooterConfiguration
{
    public class PDFFooterConfiguration
    {
        public void Apply(PageDescriptor page)
        {
            page.Footer()
                .AlignCenter()
                .Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
        }
    }
}