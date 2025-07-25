﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF.Pages
{
    /// <summary>
    /// Interaction logic for CataloguePage.xaml
    /// </summary>
    public partial class CataloguePage : Page
    {
        private readonly IVoucherTypeRepository _voucherTypeRepository;
        private List<VoucherType> _voucherTypes;
        private static readonly Regex _numericRegex = new(@"^[0-9]*(?:\.[0-9]*)?$");
        public CataloguePage()
        {
            InitializeComponent();
            _voucherTypeRepository = new VoucherTypeRepository();
            Loaded += CataloguePage_Loaded;
        }

        private void CataloguePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            // 1. Lấy tất cả VoucherType
            _voucherTypes = _voucherTypeRepository.GetAll();

            // 2. Bind lên ItemsControl
            icVouchers.ItemsSource = _voucherTypes;

            // 3. Điền các ComboBox filter
            cbCategory.ItemsSource = _voucherTypes
                .Select(v => v.Category)
                .Distinct()
                .ToList();
            cbCategory.SelectedIndex = -1;  // chưa chọn gì

            cbLocation.ItemsSource = _voucherTypes
                .Select(v => v.Location)
                .Distinct()
                .ToList();
            cbLocation.SelectedIndex = -1;
        }

        private void OnApplyFilters(object sender, RoutedEventArgs e)
        {
            // 1. Lấy giá trị keyword, category, location
            string keyword = txtSearch.Text.Trim().ToLower();
            string cat = cbCategory.SelectedItem as string;
            string loc = cbLocation.SelectedItem as string;

            // 2. Parse price range (nếu người dùng nhập)
            decimal? minPrice = null;
            decimal? maxPrice = null;

            if (!string.IsNullOrWhiteSpace(txtMinPrice.Text))
            {
                if (decimal.TryParse(txtMinPrice.Text, out var mp))
                    minPrice = mp;
                else
                {
                    MessageBox.Show("Invalid Min Price", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtMaxPrice.Text))
            {
                if (decimal.TryParse(txtMaxPrice.Text, out var xp))
                    maxPrice = xp;
                else
                {
                    MessageBox.Show("Invalid Max Price", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // 3. Lọc danh sách gốc (_voucherTypes) theo tất cả điều kiện
            var filtered = _voucherTypes.Where(v =>
                // keyword (tìm trong Name hoặc Description)
                (string.IsNullOrEmpty(keyword)
                    || v.Name.ToLower().Contains(keyword))
                // category
                && (string.IsNullOrEmpty(cat) || v.Category == cat)
                // location
                && (string.IsNullOrEmpty(loc) || v.Location == loc)
                // minPrice
                && (!minPrice.HasValue || v.DiscountValue >= minPrice.Value)
                // maxPrice
                && (!maxPrice.HasValue || v.DiscountValue <= maxPrice.Value)
            ).ToList();

            // 4. Đưa lên UI
            icVouchers.ItemsSource = filtered;

            // 5. Xóa nội dung của các bộ lọc sau khi áp dụng
            txtSearch.Clear();
            cbCategory.SelectedIndex = -1;
            cbLocation.SelectedIndex = -1;
            txtMinPrice.Clear();
            txtMaxPrice.Clear();

            // 6. Hiển thị thông báo nếu không có kết quả
            txtNoMatchMessage.Visibility = filtered.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnViewDetail(object sender, RoutedEventArgs e)
        {
            var voucher = (VoucherType)((Button)sender).Tag;
            NavigationService?.Navigate(new VoucherDetailPage(voucher));
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // giả sử đang đầy đủ chuỗi: existingText + newChar
            var textBox = (TextBox)sender;
            string full = textBox.Text.Insert(textBox.SelectionStart, e.Text);
            e.Handled = !_numericRegex.IsMatch(full);
        }

        private void NumericOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (!(e.DataObject.GetData(typeof(string)) is string paste)) return;
            var textBox = (TextBox)sender;
            string full = textBox.Text.Insert(textBox.SelectionStart, paste);
            if (!_numericRegex.IsMatch(full))
                e.CancelCommand();
        }
    }
}
