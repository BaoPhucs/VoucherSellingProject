using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VoucherSales_WPF.Converters
{
    public class StringNullOrEmptyToVisibilityConverter : IValueConverter
    {
        // nếu chuỗi rỗng hoặc null => Visible (thì show watermark),
        // ngược lại => Collapsed (ẩn đi)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            return string.IsNullOrEmpty(s)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
