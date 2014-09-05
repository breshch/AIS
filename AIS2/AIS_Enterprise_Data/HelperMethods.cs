using AIS_Enterprise_Global.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS_Enterprise_Data
{
    public class Converting
    {
        public static string DoubleToCurrency(double value, string currency)
        {
            string tmp = value.ToString("c");
            tmp = tmp.Substring(0, tmp.LastIndexOf(" "));

            return tmp + " " + currency;
        }

        public static string DoubleToCurrency(double value, Currency currency)
        {
            return DoubleToCurrency(value, currency.ToString());
        }
    }
}
