using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace ApplicationInfrastructure.Services.GenerateServices.PDFGeneratorService.PDFHeaderConfiguration
{
    public class PDFHeaderConfiguration
    {
        public void Apply(PageDescriptor page)
        {
            page.Header()
                .Text("List of Products")
                .SemiBold().FontSize(20).FontColor(Colors.Black)
                .AlignCenter();
        }
    }
}