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


        #region SelfCleaningLogging
        public static void FileLogging(int logtype, string baseFolder, string baseFile, string dataIn)
        {
            //logtype 1=forever, 2=hourly, 3=Day,
            string dataOut = DateTime.Now.ToString() + " " + dataIn + System.Environment.NewLine;
            string fileOut;
            try
            {
                if (logtype == 1)
                {
                    fileOut = baseFolder + @"\" + baseFile + ".txt";
                    System.IO.File.AppendAllText(fileOut, dataOut);
                }
                if (logtype == 2)
                {
                    fileOut = baseFolder + @"\" + baseFile + "_HOUR_" + DateTime.Now.Hour.ToString() + ".txt";

                    if (System.IO.File.Exists(fileOut))
                    {
                        if ((DateTime.Now - System.IO.File.GetLastWriteTime(fileOut)).TotalHours > 20)
                        {
                            System.IO.File.Delete(fileOut);
                        }
                    }
                    System.IO.File.AppendAllText(fileOut, dataOut);
                }
                if (logtype == 3)
                {
                    fileOut = baseFolder + @"\" + baseFile + "_DAY_" + DateTime.Now.Day.ToString() + ".txt";

                    if (System.IO.File.Exists(fileOut))
                    {
                        if ((DateTime.Now - System.IO.File.GetLastWriteTime(fileOut)).TotalDays > 2)
                        {
                            System.IO.File.Delete(fileOut);
                        }
                    }
                    System.IO.File.AppendAllText(fileOut, dataOut);
                }
            }
            catch (Exception)
            {
                //ignore Error
            }
        }
        #endregion
    }
}
