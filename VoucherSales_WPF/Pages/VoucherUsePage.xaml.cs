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
        private readonly int _voucherTypeId;

        public VoucherUsePage(int voucherTypeId)
        {
            InitializeComponent();
            DataContext = this;
            _voucherTypeId = voucherTypeId;
            Loaded += (_, __) => LoadData();
        }

        private void LoadData()
        {
            Vouchers.Clear();
            // Lấy tất cả mã unused, rồi filter theo Type:
            var list = _voucherRepo.GetMyWallet(App.CurrentUser.UserId)
                                   .Where(v => v.VoucherTypeId == _voucherTypeId);
            foreach (var v in list)
                Vouchers.Add(v);

            OnPropertyChanged(nameof(Vouchers));
        }

        private void OnConfirmUse(object sender, RoutedEventArgs e)
        {
            var sel = Vouchers.Where(v => v.IsSelected).ToList();
            if (!sel.Any())
            {
                MessageBox.Show("Hãy chọn ít nhất 1 voucher để dùng.",
                                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            foreach (var v in sel)
                _voucherRepo.Redeem(v.VoucherId);

            MessageBox.Show("Voucher(s) used!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
            // Quay lại WalletPage để reload
            NavigationService?.GoBack();
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
