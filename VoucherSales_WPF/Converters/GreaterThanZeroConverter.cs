using System;
using System.Globalization;
using System.Windows.Data;

namespace VoucherSales_WPF.Converters
{
    public class GreaterThanZeroConverter : IValueConverter
    {
        // Chỉ enable khi value > 0
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i)
                return i > 0;
            if (value is decimal d)
                return d > 0;
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
