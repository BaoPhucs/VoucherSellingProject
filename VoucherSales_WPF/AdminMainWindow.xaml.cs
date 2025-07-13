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
using VoucherSales_WPF.Manager;

namespace VoucherSales_WPF
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
            //lbAdminMenu.SelectionChanged += OnMenuChanged;
            //lbAdminMenu.SelectedIndex = 0; // Set default selection to the first item




            //private void OnMenuChanged(object s, SelectionChangedEventArgs e)
            //{
            //    switch (lbAdminMenu.SelectedIndex)
            //    {
            //        case 0: // Users
            //            ccAdminContent.Content = new ManageUsersPage();
            //            break;
            //        case 1: // VoucherTypes
            //            ccAdminContent.Content = new ManageVoucherTypesPage();
            //            break;
            //        case 2: // Reports
            //            ccAdminContent.Content = new ReportsPage();
            //            break;
            //    }
            //}
        }

        private void lbAdminMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (lbAdminMenu.SelectedItem as ListBoxItem)?.Content.ToString();

            switch (selected)
            {
                case "Quản lý Người dùng":
                    ccAdminContent.Content = new ManageUsersPage();
                    break;
                case "Quản lý Loại Voucher":
                    ccAdminContent.Content = new ManageVoucherTypesPage();
                    break;
                case "Báo cáo Doanh thu":
                    break;
            }
        }

    }
}
