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
    /// Interaction logic for PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly ICartItemRepository _cartRepo;
        private readonly IVoucherRepository _voucherRepo;
        private readonly IVoucherTypeRepository _voucherTypeRepo;

        private readonly Order _order;
        private readonly List<OrderItem> _orderItems;
        private readonly List<int> _cartItemIds;

        public Order Order { get; }
        public string OrderSummary { get; }
        public decimal Amount { get; }
        public List<string> PaymentMethods { get; }
        public string SelectedMethod { get; set; }

        public List<OrderItem> OrderItems { get; }

        public PaymentPage(Order order, List<OrderItem> orderItems, List<int> cartItemIds)
        {
            InitializeComponent();

            Order = order;
            OrderItems = orderItems;
            Amount = orderItems.Sum(i => i.Subtotal);
            //OrderSummary = $"Order #{order.OrderId} - total {Amount:C}";
            PaymentMethods = new List<string> { "Credit Card", "E-Wallet", "Cash" };
            SelectedMethod = PaymentMethods[0];



            _paymentRepo = new PaymentRepository();
            _orderRepo = new OrderRepository();
            _cartRepo = new CartItemRepository();
            _voucherRepo = new VoucherRepository();
            _voucherTypeRepo = new VoucherTypeRepository();

            _order = order;
            _orderItems = orderItems;
            _cartItemIds = cartItemIds;

            foreach (var oi in OrderItems)
            {
                oi.VoucherType = _voucherTypeRepo.GetByID(oi.VoucherTypeId);
            }

            DataContext = this;
        }

        public PaymentPage(Order order, List<OrderItem> orderItems) : this(order, orderItems, new List<int>())
        {
        }

        private void OnConfirmPayment(object sender, RoutedEventArgs e)
        {
            if (_order.OrderId == 0)
            {
                _orderRepo.CreateOrder(_order, _orderItems);
                // Sau CreateOrder, EF sẽ set Order.OrderId = giá trị identity từ DB
            }


            // 1) Lưu payment
            var payment = new Payment
            {
                OrderId = _order.OrderId,
                PaymentMethod = SelectedMethod,
                Amount = _order.TotalAmount,
                Status = "Completed",
                TransactionDate = DateTime.Now,
                TransactionRef = Guid.NewGuid().ToString()[..8].ToUpper()
            };
            _paymentRepo.CreatePayment(payment);

            // 2) Cập nhật trạng thái order
            _orderRepo.UpdateOrderStatus(_order.OrderId, "Success");

            //var voucherRepo = new VoucherRepository();
            //voucherRepo.GenerateForOrder(_order.OrderId);

            // Sinh voucher, kèm kiểm soát số lượng
            try
            {
                _voucherRepo.GenerateForOrder(_order.OrderId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể sinh mã voucher: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3) Chỉ xóa những CartItem đã được chọn
            foreach (var cartItemId in _cartItemIds)
            {
                _cartRepo.Remove(cartItemId);
            }

            MessageBox.Show("Payment successful!", "Done",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // 4) Quay về OrdersPage
            var main = Window.GetWindow(this) as CustomerMainWindow;
            main?.MainFrame.Navigate(new OrdersPage());
        }
    }
}
