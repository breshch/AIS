using Shared.Enums;

namespace AVService
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
