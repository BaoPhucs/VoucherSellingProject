using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VoucherSales_BO;
using VoucherSales_Repositories;


namespace VoucherSales_WPF.Pages
{
    /// <summary>
    /// Interaction logic for CartPage.xaml
    /// </summary>
    public partial class CartPage : Page, INotifyPropertyChanged
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CartItem> CartItems { get; set; } = new();
        public CartPage()
        {
            InitializeComponent();
            _cartItemRepository = new CartItemRepository();
            _orderRepository = new OrderRepository();

            Loaded += (_, __) => LoadCart();
            DataContext = this;
        }


        private void LoadCart()
        {
            var list = _cartItemRepository.GetByUser(App.CurrentUser.UserId);

            // Reset lại collection
            CartItems = new ObservableCollection<CartItem>(list);

            // Subscribe mỗi item để lắng nghe Quantity/Subtotal thay đổi
            foreach (var ci in CartItems)
                ci.PropertyChanged += CartItem_PropertyChanged;

            // Gán lại DataGrid
            dgCart.ItemsSource = CartItems;

            RaisePropertyChanged(nameof(CartItems));
            RaisePropertyChanged(nameof(CartTotal));
        }

        private void CartItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is CartItem ci)
            {
                if (e.PropertyName == nameof(CartItem.Quantity))
                {
                    // ... cập nhật DB và subtotal như trước ...
                    dgCart.Items.Refresh();
                }
                // **Mới**: khi IsSelected thay đổi thì cập nhật tổng
                if (e.PropertyName == nameof(CartItem.IsSelected))
                {
                    RaisePropertyChanged(nameof(CartTotal));
                }
            }
        }

        // Tính tổng động
        public decimal CartTotal => CartItems.Where(ci => ci.IsSelected)
                .Sum(ci => ci.Subtotal);

        // Remove riêng 1 item
        private void OnRemoveFromCart(object sender, RoutedEventArgs e)
        {
            int cartItemId = (int)((Button)sender).Tag;
            _cartItemRepository.Remove(cartItemId);
            LoadCart();
        }

        // Checkout chỉ với những hàng đã chọn
        private void OnCheckout(object sender, RoutedEventArgs e)
        {
            var toCheckout = CartItems.Where(ci => ci.IsSelected).ToList();
            if (!toCheckout.Any())
            {
                MessageBox.Show("Please tick at least one item to checkout.",
                                "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var cartItemIds = toCheckout.Select(ci => ci.CartItemId).ToList();
            // 2) Map thành OrderItem
            var orderItems = toCheckout.Select(ci => new OrderItem
            {
                VoucherTypeId = ci.VoucherTypeId,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Subtotal = ci.Subtotal
            }).ToList();

            // 3) Tạo Order (chưa cập nhật PaymentStatus thành Success)
            var order = new Order
            {
                UserId = App.CurrentUser.UserId,
                OrderDate = DateTime.Now,             // nếu bạn dùng CreatedAt thay vì OrderDate
                TotalAmount = orderItems.Sum(oi => oi.Subtotal),
                PaymentStatus = "Pending",
                Notes = "Purchased via Cart"
            };
            //_orderRepository.CreateOrder(order, orderItems);

            // 4) Chuyển vào PaymentPage để chọn phương thức và xác nhận
            var main = Window.GetWindow(this) as CustomerMainWindow;
            main?.MainFrame.Navigate(new PaymentPage(order, orderItems, cartItemIds));
        }

        private void RaisePropertyChanged(string prop)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
