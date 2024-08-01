using ApplicationCore.Domain.Entity.Ceneo;
using Applications.Handles;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Applications.Services.CeneoService
{
    public class SearchProductCeneoService:ISearchProductCeneoService
    {
        private const string SearchUrlTemplate = "https://www.ceneo.pl/;szukaj-{0}";
        private const string BaseUrl = "https://www.ceneo.pl";


        public CeneoProduct Search(string productName)
        {
            string searchUrl = string.Format(SearchUrlTemplate, WebUtility.UrlEncode(productName));

            var web = new HtmlWeb();

            var searchDocument = web.Load(searchUrl);

            var productLink = searchDocument.DocumentNode
                .SelectNodes("//a[contains(@class, 'go-to-product js_conv')]")
                .FirstOrDefault(node => IsExactMatch(node.InnerText.Trim(), productName));

            if (productLink != null)
            {

                string productUrl = BaseUrl + productLink.GetAttributeValue("href", "");
                var productDocument = web.Load(productUrl);

                var mainPriceNode = productDocument.DocumentNode.SelectSingleNode("//*[@id='body']/div[2]/div/div/article/div/div[2]/div/div/div[1]/span/span");
                string mainPrice = mainPriceNode != null ? mainPriceNode.InnerText.Trim() : "Main price not found";

                var products = GetProducts(productDocument);

                if (products.Any())
                {
                    var Product = new CeneoProduct
                    {
                        ProductName = productName,
                        PriceCeneo = CeneoProductInformationHandlers.ParsePrice(mainPrice),
                        productInformation = products
                    };
                    return Product;
                }
                else
                {
                    throw new Exception("No products found on Ceneo.");
                }
            }
            else
            {
                throw new Exception("Product not found on Ceneo.");
            }            
        }
        private static List<CeneoProductInformation> GetProducts(HtmlDocument document)
        {
            var products = new List<CeneoProductInformation>();
            
            // Get prices and details from section 1
            var products1 = GetProductsInSection(document, "//*[@id='click']/div[2]/section[1]/ul/li");

            // Get prices and details from section 2
            var products2 = GetProductsInSection(document, "//*[@id='click']/div[2]/section[2]/ul/li");

            products.AddRange(products1);
            products.AddRange(products2);

            return products;
        }
        private static List<CeneoProductInformation> GetProductsInSection(HtmlDocument document, string xpath)
        {
            var productNodes = document.DocumentNode.SelectNodes(xpath);
            var products = new List<CeneoProductInformation>();

            if (productNodes != null)
            {
                foreach (var node in productNodes)
                {
                    var detailsNode = node.SelectSingleNode(".//div[1]/div[1]/a/span");
                    var priceNode = node.SelectSingleNode(".//div[2]/div[2]/a/span/span");
                    var ocenaNode = node.SelectSingleNode(".//div[1]/div[2]/span[1]/span[2]");
                    var ilostOpinjiNode = node.SelectSingleNode(".//div[1]/div[2]/span[2]");

                    if (detailsNode != null && priceNode != null)
                    {
                        var details = detailsNode.InnerText.Trim();
                        var price = priceNode.InnerText.Trim();
                        var ocena = ocenaNode?.InnerText.Trim();
                        var ilostOpinji = ilostOpinjiNode?.InnerText.Trim();

                        products.Add(new CeneoProductInformation
                        {
                            Details = details,
                            Price = CeneoProductInformationHandlers.ParsePrice(price),
                            Ocena = CeneoProductInformationHandlers.ParseOcena(ocena!),
                            IlostOpinji =CeneoProductInformationHandlers.ParseIlostOpinji(ilostOpinji!)
                        });
                    }
                }
            }

            return products;
        }
        private static bool IsExactMatch(string text, string productName)
        {
            if (string.Equals(text, productName))
            {
                return true;
            }
            return productName.Contains(text) || text.Contains(productName);
        }
    }
}
