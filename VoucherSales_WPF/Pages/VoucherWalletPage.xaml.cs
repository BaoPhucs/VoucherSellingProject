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
    /// Interaction logic for VoucherWalletPage.xaml
    /// </summary>
    public partial class VoucherWalletPage : Page, INotifyPropertyChanged
    {
        private readonly IVoucherWalletRepository _walletRepo = new VoucherWalletRepository();
        private readonly IVoucherRedemptionRepository _redRepo = new VoucherRedemptionRepository();

        public ObservableCollection<VoucherType> Wallet { get; set; } = new();
        public ObservableCollection<VoucherRedemption> Redemptions { get; set; } = new();

        public VoucherWalletPage()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += (_, __) => RefreshAll();
        }

        private void RefreshAll()
        {
            // 1) Load wallet summary
            Wallet.Clear();
            var w = _walletRepo.GetWalletForUser(App.CurrentUser.UserId);
            foreach (var vt in w)
                Wallet.Add(vt);

            // 2) Load redemption history
            Redemptions.Clear();
            var r = _redRepo.GetByUser(App.CurrentUser.UserId);
            foreach (var rem in r)
                Redemptions.Add(rem);

            OnPropertyChanged(nameof(Wallet));
            OnPropertyChanged(nameof(Redemptions));
        }

        // Khi user click Use
        private void OnUse(object sender, RoutedEventArgs e)
        {
            int voucherTypeId = (int)((Button)sender).Tag;
            // Navigate sang trang Use voucher
            NavigationService?.Navigate(new VoucherUsePage(voucherTypeId));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}