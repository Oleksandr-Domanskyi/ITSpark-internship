using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Handles
{
    public class CeneoProductInformationHandlers
    {
        public static decimal ParseOcena(string ocenaString)
        {

            int startIndex = ocenaString.IndexOf("Ocena ") + 6;
            int endIndex = ocenaString.IndexOf(" /");

            ocenaString = ocenaString.Substring(startIndex, endIndex - startIndex);

            ocenaString = ocenaString.Replace(",", ".");

            if (decimal.TryParse(ocenaString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal rating))
            {
                return rating;
            }
            else
            {
                throw new ArgumentException("ParsingErrors: Ocena");
            }
        }
        public static int ParseIlostOpinji(string opinjiString)
        {
            int endIndex = opinjiString.IndexOf(" ");

            string opinjiCount = opinjiString.Substring(0, endIndex);

            if (int.TryParse(opinjiCount, NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("ParsingErrors: IlostOpinji");
            }
        }
        public static decimal ParsePrice(string priceString)
        {
            priceString = priceString.Replace(",", ".");

            if (decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("ParsingErrors: Price");
            }
        }
    }
}
