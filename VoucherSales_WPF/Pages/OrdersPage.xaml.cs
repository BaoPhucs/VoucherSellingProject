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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF.Pages
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private readonly IOrderRepository _orderRepository;
        public ObservableCollection<Order> Orders { get; set; } = new();
        public OrdersPage()
        {
            InitializeComponent();
            _orderRepository = new OrderRepository();
            Loaded += (_, __) => LoadOrders();
            DataContext = this;
        }

        private void LoadOrders()
        {
            var list = _orderRepository.GetByUser(App.CurrentUser.UserId);
            Orders.Clear();
            foreach (var o in list)
                Orders.Add(o);
        }

        private void OnViewOrderDetail(object sender, RoutedEventArgs e)
        {
            int orderId = (int)((Button)sender).Tag;
            var main = Window.GetWindow(this) as CustomerMainWindow;
            main?.MainFrame.Navigate(new OrderDetailPage(orderId));
        }
    }
}
