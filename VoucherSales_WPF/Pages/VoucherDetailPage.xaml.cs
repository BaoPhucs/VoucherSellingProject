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
    /// Interaction logic for VoucherDetailPage.xaml
    /// </summary>
    public partial class VoucherDetailPage : Page
    {
        public VoucherType SelectedVoucher { get; set; }
        private readonly IOrderRepository _orderRepository;
        private readonly ICartItemRepository _cartItemRepository;
        public int SelectedQuantity { get; set; } = 1;
        public VoucherDetailPage(VoucherType voucher)
        {
            InitializeComponent();

            SelectedVoucher = voucher;
            SelectedVoucher = voucher;
            DataContext = this;
            _cartItemRepository = new CartItemRepository();
            _orderRepository = new OrderRepository();
        }

        private void OnAddToCart(object sender, RoutedEventArgs e)
        {
            // 1) Thêm vào cart
            _cartItemRepository.AddOrUpdate(
                App.CurrentUser.UserId,
                SelectedVoucher.VoucherTypeId,
                SelectedQuantity);

            // 2) Hỏi xem có chuyển sang CartPage không
            var result = MessageBox.Show(
                "Added to cart successfully.\n\nDo you want to go to your Cart now?",
                "Success",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                // 3) Navigate vào CartPage
                // Nếu page này đang được host trong một Frame của CustomerMainWindow
                var main = Window.GetWindow(this) as CustomerMainWindow;
                if (main != null)
                {
                    main.MainFrame.Navigate(new CartPage());
                }
                else if (NavigationService != null)
                {
                    // fallback nếu chỉ có NavigationService
                    NavigationService.Navigate(new CartPage());
                }
            }
            // nếu No thì cứ ở lại VoucherDetailPage
        }

        private void OnBuyNow(object sender, RoutedEventArgs e)
        {
            // 1) Map thông tin thành OrderItem
            var orderItems = new List<OrderItem>
            {
                new OrderItem {
                    VoucherTypeId = SelectedVoucher.VoucherTypeId,
                    Quantity      = SelectedQuantity,
                    UnitPrice     = SelectedVoucher.DiscountValue,
                    Subtotal      = SelectedVoucher.DiscountValue * SelectedQuantity
                }
            };

            // 2) Tạo Order (Pending)
            var order = new Order
            {
                UserId = App.CurrentUser.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = orderItems.Sum(x => x.Subtotal),
                PaymentStatus = "Pending",
                Notes = $"BuyNow: {SelectedVoucher.Name} x{SelectedQuantity}"
            };

            // 3) Navigate sang PaymentPage (dùng overload không xóa cart)
            var main = Window.GetWindow(this) as CustomerMainWindow;
            main?.MainFrame.Navigate(new PaymentPage(order, orderItems));
        }

        private void OnBackToCatalogue(object sender, RoutedEventArgs e)
        {
            // Nếu đang chạy trong Frame của CustomerMainWindow:
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                // Fallback: navigate trực tiếp về CataloguePage
                var main = Window.GetWindow(this) as CustomerMainWindow;
                main?.MainFrame.Navigate(new CataloguePage());
            }
        }
    }
}
