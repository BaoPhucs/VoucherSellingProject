using System.Globalization;
using System.Windows.Controls;

namespace VoucherSales_WPF.ValidationRules
{
    public class PositiveIntRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse((value ?? "").ToString(), out int i) && i > 0)
                return ValidationResult.ValidResult;
            return new ValidationResult(false, "Phải là số nguyên > 0");
        }
    }
}
