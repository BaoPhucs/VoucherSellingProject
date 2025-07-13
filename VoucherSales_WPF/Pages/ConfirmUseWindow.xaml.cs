using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using VoucherSales_BO;

namespace VoucherSales_WPF.Pages
{
    /// <summary>
    /// Interaction logic for ConfirmUseWindow.xaml
    /// </summary>
    public partial class ConfirmUseWindow : Window
    {
        public ObservableCollection<VoucherItemVM> VouchersToConfirm { get; }
        public ConfirmUseWindow(IEnumerable<VoucherItemVM> items)
        {
            InitializeComponent();
            VouchersToConfirm = new ObservableCollection<VoucherItemVM>(items);
            DataContext = this;
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
