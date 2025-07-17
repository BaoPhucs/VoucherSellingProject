using System.Collections.ObjectModel;
using System.Windows;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF
{
    public partial class OrderDetailWindow : Window
    {
        public ObservableCollection<OrderItem> OrderItems { get; set; }
        public Order CurrentOrder { get; set; }
        public string OrderInfo => $"Order #{CurrentOrder.OrderId} for {CurrentOrder.User.FullName}";

        private readonly IOrderRepository _orderRepo;

        public OrderDetailWindow(Order order, IOrderRepository orderRepo)
        {
            InitializeComponent();
            CurrentOrder = order;
            OrderItems = new ObservableCollection<OrderItem>(order.OrderItems);
            _orderRepo = orderRepo;
            DataContext = this;
        }


        private void OrderItemGrid_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == System.Windows.Controls.DataGridEditAction.Commit)
            {
                if (e.Row.Item is OrderItem item)
                {
                    if (item.OrderItemId == 0) // New item
                    {
                        // Wrap the single OrderItem into a List<OrderItem> as required by the method signature
                        _orderRepo.CreateOrderItem(item);
                        OrderItems.Add(item);
                        MessageBox.Show("Order item added.");
                    }
                    else // Existing item
                    {
                        _orderRepo.UpdateOrder(item.Order, new List<OrderItem> { item });
                        MessageBox.Show("Order item updated.");
                    }
                }
            }
        }

     

        

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrderItemGrid.SelectedItem is OrderItem selected)
            {

                _orderRepo.DeleteOrderItem(selected.OrderItemId); // Remove from DB
                OrderItems.Remove(selected);
                MessageBox.Show("Order item deleted.");
            }
        }



        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OrderItemGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //getAllItems of an order then show to the grid
            OrderItems.Clear();
            foreach (var item in _orderRepo.GetOrderItemsByOrderId(CurrentOrder.OrderId))
            {
                OrderItems.Add(item);
            }

            OrderItemGrid.ItemsSource = OrderItems;

        }
    }
}
