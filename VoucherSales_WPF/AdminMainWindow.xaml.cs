using System.Windows;
using System.Windows.Controls;
using VoucherSales_WPF.Manager;

namespace VoucherSales_WPF
{
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
            lbAdminMenu.SelectedIndex = 0;
        }

        private void lbAdminMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAdminMenu.SelectedItem is not ListBoxItem selectedItem)
                return;

            string selected = selectedItem.Content.ToString();

            switch (selected)
            {
                case "Quản lý Người dùng":
                    ccAdminContent.Content = new ManageUsersPage();
                    break;

                case "Quản lý Loại Voucher":
                    ccAdminContent.Content = new ManageVoucherTypesPage();
                    break;

                case "Báo cáo Doanh thu":
                    ccAdminContent.Content = new RevenueReportPage();
                    break;

                default:
                    ccAdminContent.Content = null;
                    break;
            }
        }
    }
}
