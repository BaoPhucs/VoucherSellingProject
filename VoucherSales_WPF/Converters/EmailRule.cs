using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VoucherSales_WPF.Converters
{
    public class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var email = (value ?? string.Empty).ToString().Trim();

            if (string.IsNullOrEmpty(email))
                return new ValidationResult(false, "Email không được để trống.");

            // Regex đơn giản để kiểm tra có dạng abc@xyz.tld
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
                return new ValidationResult(false, "Email không hợp lệ.");

            return ValidationResult.ValidResult;
        }
    }
}
