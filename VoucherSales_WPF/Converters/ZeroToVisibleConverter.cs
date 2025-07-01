using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VoucherSales_WPF.Converters
{
    public class ZeroToVisibleConverter : IValueConverter
    {
        // nếu Count == 0 => Visible (show empty-state),
        // ngược lại => Collapsed
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i && i == 0)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
