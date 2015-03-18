using System;
using System.Globalization;
using System.Windows.Data;

namespace AIS_Enterprise_Global.Helpers.ValueConverters
{
    public class ValueToBooleanValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? bool.Parse("True") : bool.Parse("False");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
