using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Azure
{
    public class AzureOptions
    {
        public string? BlobURL { get; set; }
        public string? ResourceGroup { get; set; }
        public string? Account { get; set; }
        public string? Container { get; set; }
        public string? ConnectionString { get; set; }
    }
}