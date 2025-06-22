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
using VoucherSales_Repositories;
using VoucherSales_WPF.Pages;

namespace VoucherSales_WPF
{
    /// <summary>
    /// Interaction logic for CustomerMainWindow.xaml
    /// </summary>
    public partial class CustomerMainWindow : Window
    {
        private readonly ICartItemRepository _cartItemRepository;
        public CustomerMainWindow(VoucherSales_BO.User user)
        {
            InitializeComponent();
            _cartItemRepository = new CartItemRepository();
            Loaded += (s, e) => MainFrame.Navigate(new CataloguePage());

            // Gán sự kiện chuyển Page
            btnCatalogue.Click += (s, e) => MainFrame.Navigate(new CataloguePage());
            btnCart.Click += (s, e) => MainFrame.Navigate(new CartPage());
            btnOrders.Click += (s, e) => MainFrame.Navigate(new OrdersPage());
            btnOrders.Click += (s, e) => MainFrame.Navigate(new OrdersPage());

            // Logout: quay về login
            btnLogout.Click += (s, e) =>
            {
                var result = MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Confirm Logout",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );
                if (result == MessageBoxResult.Yes)
                {
                    new Login().Show();
                    this.Close();
                }
            };
        }

    }
}
