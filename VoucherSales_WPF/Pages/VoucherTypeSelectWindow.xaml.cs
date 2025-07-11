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
using System.Windows.Shapes;
using VoucherSales_BO;

namespace VoucherSales_WPF.Pages
{
    /// <summary>  
    /// Interaction logic for VoucherTypeSelectWindow.xaml  
    /// </summary>  
    public partial class VoucherTypeSelectWindow : Window
    {
        public VoucherType? SelectedVoucherType { get; private set; }
        
        public VoucherTypeSelectWindow(List<VoucherType> voucherTypes)
        {
            InitializeComponent();
           
            VoucherTypeListBox.ItemsSource = voucherTypes;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (VoucherTypeListBox.SelectedItem is VoucherType selected)
            {
                SelectedVoucherType = selected;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a voucher type.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
