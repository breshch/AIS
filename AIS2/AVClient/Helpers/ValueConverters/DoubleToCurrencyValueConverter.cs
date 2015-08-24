using System;
using System.Globalization;
using System.Windows.Data;

namespace AVClient.Helpers.ValueConverters
{
    public class DoubleToCurrencyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double cash = ((double)value);
            return cash != 0 ? cash.ToString("c") : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
