using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for VoucherUsePage.xaml
    /// </summary>
    public partial class VoucherUsePage : Page, INotifyPropertyChanged
    {
        private readonly IVoucherRepository _voucherRepo = new VoucherRepository();

        public ObservableCollection<Voucher> Vouchers { get; set; } = new();
        public ObservableCollection<string> Locations { get; set; } = new();
        private readonly int _voucherTypeId;

        private string? _selectedLocation;
        public string? SelectedLocation
        {
            get => _selectedLocation;
            set { _selectedLocation = value; OnPropertyChanged(nameof(SelectedLocation)); }
        }
        public VoucherUsePage(int voucherTypeId)
        {
            InitializeComponent();
            DataContext = this;
            _voucherTypeId = voucherTypeId;
            Loaded += (_, __) => {
                LoadLocations();
                LoadData();
            };
        }

        private void LoadData()
        {
            Vouchers.Clear();
            // Lấy tất cả mã unused, rồi filter theo Type:
            var list = _voucherRepo.GetMyWalletVouchers(App.CurrentUser.UserId)
                                   .Where(v => v.VoucherTypeId == _voucherTypeId);
            foreach (var v in list)
                Vouchers.Add(v);

            OnPropertyChanged(nameof(Vouchers));
        }

        private void LoadLocations()
        {
            // Ví dụ: lấy tất cả location từ voucher types
            var locs = _voucherRepo
                          .GetMyWalletVouchers(App.CurrentUser.UserId)
                          .Select(v => v.VoucherType.Location)
                          .Distinct()
                          .ToList();
            Locations.Clear();
            foreach (var l in locs) Locations.Add(l);
            // Không bắt buộc chọn ngay
            SelectedLocation = Locations.FirstOrDefault();
        }

        //private void OnConfirmUse(object sender, RoutedEventArgs e)
        //{
        //    var sel = Vouchers.Where(v => v.IsSelected).ToList();
        //    if (!sel.Any())
        //    {
        //        MessageBox.Show("Hãy chọn ít nhất 1 voucher để dùng.",
        //                        "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    if (string.IsNullOrWhiteSpace(SelectedLocation))
        //    {
        //        MessageBox.Show("Please choose a location.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    foreach (var v in sel)
        //        _voucherRepo.Redeem(v.VoucherId, SelectedLocation);

        //    MessageBox.Show("Voucher(s) used!", "Success",
        //                    MessageBoxButton.OK, MessageBoxImage.Information);
        //    // Quay lại WalletPage để reload
        //    NavigationService?.GoBack();
        //}

        private void OnConfirmUse(object sender, RoutedEventArgs e)
        {
            // 1) Lấy danh sách voucher đã tick
            var sel = Vouchers
                        .Where(v => v.IsSelected)
                        .ToList();
            if (!sel.Any())
            {
                MessageBox.Show("Hãy chọn ít nhất 1 voucher để dùng.",
                                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedLocation))
            {
                MessageBox.Show("Vui lòng chọn Location.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2) Chuyển sang view-model để show trong popup
            var vmList = sel
              .Select(v => new VoucherItemVM
              {
                  VoucherId = v.VoucherId,
                  Code = v.Code,
                  ValidTo = v.VoucherType.ValidTo,
                  Location = v.VoucherType.Location,
                  Category = v.VoucherType.Category,
                  VoucherTypeName = v.VoucherType.Name
              })
              .ToList();

            // 3) Khởi tạo và ShowDialog modal
            var confirmDlg = new VoucherSales_WPF.Pages.ConfirmUseWindow(vmList)
            {
                Owner = Window.GetWindow(this)
            };
            bool? confirmed = confirmDlg.ShowDialog();
            if (confirmed != true)
            {
                // người dùng bấm Hủy — không làm gì thêm
                return;
            }

            // 4) Nếu đồng ý, mới thực thi logic cũ
            foreach (var v in sel)
            {
                _voucherRepo.Redeem(v.VoucherId, SelectedLocation);
                Vouchers.Remove(v);   // hoặc reload lại dataset
            }

            MessageBox.Show("Sử dụng voucher thành công!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void OnCancel(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
