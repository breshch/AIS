﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace AIS_Enterprise_AV.Helpers.ValueConverters
{
    public class BooleanToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Да" : "Нет";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
