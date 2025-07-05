using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private List<Order> _allOrders;
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
            // 1) LẤY và lưu danh sách gốc
            _allOrders = _orderRepository
                            .GetByUser(App.CurrentUser.UserId)
                            .ToList();

            // 2) Hiển thị lên UI
            Orders.Clear();
            foreach (var o in _allOrders)
                Orders.Add(o);
        }

        //private void OnOrderSearch(object sender, TextChangedEventArgs e)
        //{
        //    // Nếu _allOrders chưa được load thì bỏ qua
        //    if (_allOrders == null) return;

        //    string kw = txtOrderSearch.Text.Trim();
        //    var filtered = _allOrders
        //        .Where(o => o.OrderId.ToString().Contains(kw))
        //        .ToList();

        //    Orders.Clear();
        //    foreach (var o in filtered)
        //        Orders.Add(o);
        //}

        private void OnOrderSearch(object sender, TextChangedEventArgs e)
        {
            if (_allOrders == null) return;

            string kw = txtOrderSearch.Text.Trim();

            // Chuẩn bị: cố gắng parse kw thành DateTime hoặc decimal
            DateTime dt;
            bool isDate = DateTime.TryParseExact(kw,
                     new[] { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd" },
                     CultureInfo.CurrentCulture,
                     DateTimeStyles.None,
                     out dt);

            decimal amount;
            bool isAmount = decimal.TryParse(kw, NumberStyles.Currency, CultureInfo.CurrentCulture, out amount);

            var filtered = _allOrders.Where(o =>
                // 1) tìm theo OrderId
                o.OrderId.ToString().Contains(kw)
                // 2) tìm theo ngày (nếu kw parse được DateTime hoặc so sánh chuỗi)
                || (isDate && o.OrderDate.Date == dt.Date)
                || o.OrderDate.ToString("dd/MM/yyyy").Contains(kw)
                // 3) tìm theo tổng tiền (nếu kw parse được số)
                || (isAmount && o.TotalAmount == amount)
                || o.TotalAmount.ToString("C").Contains(kw)
            ).ToList();

            Orders.Clear();
            foreach (var o in filtered)
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
