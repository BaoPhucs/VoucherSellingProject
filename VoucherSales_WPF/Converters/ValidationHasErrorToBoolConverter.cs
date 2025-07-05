using System;
using System.Globalization;
using System.Windows.Data;

namespace VoucherSales_WPF.Converters
{
    /// <summary>
    /// Chuyển Validation.HasError (bool) -> !HasError để bind IsEnabled
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ValidationHasErrorToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
