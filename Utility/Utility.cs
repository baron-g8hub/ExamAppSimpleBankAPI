using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Utility
    {
        public static string HandleAmountString(string amount)
        {
            double.TryParse(amount, out double result);

            string amountStr = string.Format("{0:F2}", result);
            //amountStr = string.Format("{0:C2}", 5.9d); //results in $5.90
            //amountStr = string.Format("{0:C2}", 5.123d); //results in $5.12
            return amountStr;
        }


        public static string FormatPriceString(string price)
        {
            string retPriceStr;
            if (double.Parse(price) < 9)
            {
                retPriceStr = "₱        " + HandleAmountString(price);
            }
            else if (double.Parse(price) < 99)
            {
                retPriceStr = "₱      " + HandleAmountString(price);
            }
            else if (double.Parse(price) < 999)
            {
                retPriceStr = "₱    " + HandleAmountString(price);
            }
            else if (double.Parse(price) < 9999)
            {
                retPriceStr = "₱  " + HandleAmountString(price);
            }
            else
            {
                retPriceStr = "₱  " + HandleAmountString(price);
            }
            return retPriceStr;
        }
    }
}
