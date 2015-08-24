using System;
using System.Globalization;
using System.Windows.Data;

namespace AVClient.Helpers.ValueConverters
{
    public class PostToShortPostValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string postName = value.ToString();
            if (postName.IndexOf('_') != -1)
            {
                postName = postName.Substring(0, postName.IndexOf('_'));
            }

            return postName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
