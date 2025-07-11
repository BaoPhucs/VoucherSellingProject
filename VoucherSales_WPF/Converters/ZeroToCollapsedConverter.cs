﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VoucherSales_WPF.Converters
{
    public class ZeroToCollapsedConverter : IValueConverter
    {
        // nếu Count == 0 => Collapsed,
        // ngược lại => Visible
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i && i == 0)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
