using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace VoucherSales_WPF.ValidationRules
{
    public class PhoneRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var phone = (value ?? string.Empty).ToString().Trim();

            if (string.IsNullOrEmpty(phone))
                return new ValidationResult(false, "Số điện thoại không được để trống.");

            // Ví dụ: chỉ cho phép 9–15 chữ số
            var pattern = @"^\d{9,15}$";
            if (!Regex.IsMatch(phone, pattern))
                return new ValidationResult(false, "Số điện thoại không hợp lệ.");

            return ValidationResult.ValidResult;
        }
    }
}
