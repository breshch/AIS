using System;
using System.Globalization;
using System.Windows.Data;
using AIS_Enterprise_Global.Helpers;

namespace AIS_Enterprise_AV.Helpers.ValueConverters
{
    public class GenderToFemaleBoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Gender gender = (Gender) Enum.Parse(typeof(Gender), value.ToString());

            return gender == Gender.Female;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return bool.Parse(value.ToString()) ? Gender.Female : Gender.Male;
        }
    }
}
