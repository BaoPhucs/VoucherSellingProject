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

        private readonly IOrderRepository _orderRepo;

        public OrderDetailWindow(Order order, IOrderRepository orderRepo)
        {
            InitializeComponent();
            CurrentOrder = order;
            OrderItems = new ObservableCollection<OrderItem>(order?.OrderItems ?? new List<OrderItem>());
            _orderRepo = orderRepo;
            //_allOrderItems = order.OrderItems?.ToList() ?? new List<OrderItem>();

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
                        try
                        {
                            _orderRepo.CreateOrderItem(item);
                            _orderRepo.UpdateOrderTotal(CurrentOrder.OrderId); // Update the total amount of the order
                            MessageBox.Show("Order item added.");
                            CurrentOrder = _orderRepo.GetByID(CurrentOrder.OrderId); // Refresh order

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error adding order item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else // Existing item
                    {
                        try
                        {
                            _orderRepo.UpdateOrderItem(item);
                            // Update the order with the modified item
                            _orderRepo.UpdateOrderTotal(CurrentOrder.OrderId); // Update the total amount of the order
                            MessageBox.Show("Order item updated.");
                            CurrentOrder = _orderRepo.GetByID(CurrentOrder.OrderId); // Refresh order

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error updating order item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }





        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrderItemGrid.SelectedItem is OrderItem selected)
            {
                try
                {
                    _orderRepo.DeleteOrderItem(selected.OrderItemId); // Remove from DB
                    OrderItems.Remove(selected); // Remove from the ObservableCollection

                    // Update the order with remaining items
                    _orderRepo.UpdateOrder(CurrentOrder, OrderItems.ToList());
                    CurrentOrder = _orderRepo.GetByID(CurrentOrder.OrderId); // Refresh order

                    MessageBox.Show("Order item deleted.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting order item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        //private List<OrderItem> _allOrderItems;


        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            

            string searchText = txtSearchOrderItem.Text?.Trim().ToLower() ?? string.Empty;

            // Defensive: ensure _orderRepo and CurrentOrder are not null
            if (_orderRepo == null || CurrentOrder.OrderId == 0)
                return;

            var orderItems = _orderRepo.GetOrderItemsByOrderId(CurrentOrder.OrderId);
            if (orderItems == null)
                return;

            var filtered = orderItems
                .Where(item =>
                    (item?.VoucherType?.VoucherTypeId.ToString().Contains(searchText) ?? false) ||
                    item?.OrderItemId.ToString().Contains(searchText) == true
                ).ToList();

            OrderItems.Clear();
            foreach (var item in filtered)
                OrderItems.Add(item); //add to list for loading
        }
    }
}
