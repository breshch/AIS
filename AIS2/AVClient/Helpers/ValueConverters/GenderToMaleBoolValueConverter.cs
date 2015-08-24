using System;
using System.Globalization;
using System.Windows.Data;
using AVClient.AVServiceReference;

namespace AVClient.Helpers.ValueConverters
{
    public class GenderToMaleBoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Gender gender = (Gender) Enum.Parse(typeof(Gender), value.ToString());

            return gender == Gender.Male;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bool.Parse(value.ToString()) ? Gender.Male : Gender.Female;
        }
    }
}
