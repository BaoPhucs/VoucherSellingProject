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
    /// Interaction logic for OrderDetailPage.xaml
    /// </summary>
    public partial class OrderDetailPage : Page
    {
        private readonly IOrderRepository _orderRepo = new OrderRepository();
        private readonly IPaymentRepository _paymentRepo = new PaymentRepository();

        public Order CurrentOrder { get; set; }
        public Payment CurrentPayment { get; set; }

        public OrderDetailPage(int orderId)
        {
            InitializeComponent();
            CurrentOrder = _orderRepo.GetByID(orderId);
            CurrentPayment = _paymentRepo.GetByOrder(orderId)
                             ?? new Payment { Status = "Pending" };
            DataContext = this;
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
            else
            {
                // Fallback: trở về trang Orders
                var main = Window.GetWindow(this) as CustomerMainWindow;
                main?.MainFrame.Navigate(new OrdersPage());
            }
        }
    }
}

