using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string keyword = txtSearch.Text.Trim().ToLower();
            string cat = cbCategory.SelectedItem as string;
            string loc = cbLocation.SelectedItem as string;

            var filtered = _voucherTypes.Where(v =>
                (string.IsNullOrEmpty(keyword)
                    || v.Name.ToLower().Contains(keyword)
                    || (v.Description?.ToLower().Contains(keyword) ?? false))
                && (string.IsNullOrEmpty(cat) || v.Category == cat)
                && (string.IsNullOrEmpty(loc) || v.Location == loc)
            ).ToList();

            icVouchers.ItemsSource = filtered;
        }

        private void OnViewDetail(object sender, RoutedEventArgs e)
        {
            var voucher = (VoucherType)((Button)sender).Tag;
            NavigationService?.Navigate(new VoucherDetailPage(voucher));
        }
    }
}
