using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using VoucherSales_BO;
using VoucherSales_DAO;
using VoucherSales_Repositories;
using VoucherSales_WPF.Pages;

namespace VoucherSales_WPF
{
    public partial class StaffMainWindow : Window
    {
        private readonly IVoucherRepository _voucherRepo = new VoucherRepository();
        private readonly IOrderRepository _orderRepo = new OrderRepository();
        private readonly IUserRepository _userRepo = new UserRepository();
        private readonly IVoucherTypeRepository _voucherTypeRepo = new VoucherTypeRepository();
        private List<Voucher> _currentVouchers = new List<Voucher>();
        private List<Order> _currentOrders = new List<Order>();
        private List<VoucherType> _voucherTypes = new List<VoucherType>();
        private List<User> _users = new List<User>();

        // Add a field to store the logged-in user
        private readonly User _loggedInUser;

        public StaffMainWindow()
        {
            InitializeComponent();
            LoadUsers();
            LoadMetaData();
            SetStaffInfo();
        }


        public StaffMainWindow(User loggedInUser)
        {
            _loggedInUser = loggedInUser;
            InitializeComponent();
            LoadUsers();
            LoadMetaData();
            SetStaffInfo();
        }

        private void SetStaffInfo()
        {
            if (_loggedInUser != null)
            {
                StaffInfoTextBlock.Text = $"Staff: {_loggedInUser.UserId}, {_loggedInUser.FullName}";
            }


           
        }
        private void LoadMetaData()
        {
            _voucherTypes = _voucherTypeRepo.GetAll();
            // Assume PaymentStatusList is defined in resources
            Resources["PaymentStatusList"] = new List<string> { "Pending", "Success", "Cancelled" };
        }
        private void LoadUsers()
        {
            var users = _userRepo.GetAllUsers();
            UserComboBox.ItemsSource = users;
            if (users.Count > 0)
                UserComboBox.SelectedIndex = 0;
        }

        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadVouchers();
            LoadOrders();
        }

        // load *all* vouchers of the current user, not just unredeemed

        private void LoadVouchers()
        {
            if (UserComboBox.SelectedValue is int userId)
            {
                _currentVouchers = _voucherRepo.GetMyWalletVouchers(userId);

                VoucherGrid.ItemsSource = _currentVouchers;
            }
        }


        private void EditVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (VoucherGrid.SelectedItem is Voucher selected)
            {
                try
                {
                    selected.IsRedeemed = !selected.IsRedeemed;
                    _voucherRepo.Redeem(selected.VoucherId, null);
                    LoadVouchers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Voucher Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a voucher to save changes.");
            }
        }

        private void DeleteVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (VoucherGrid.SelectedItem is Voucher selected)
            {
                //remove
                _voucherRepo.DeleteVoucher(selected.VoucherId);
                MessageBox.Show("Voucher deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadVouchers();
            }
            else
            {
                MessageBox.Show("Please select a voucher to delete.");
            }
        }

        private void LoadOrders()
        {
            if (UserComboBox.SelectedValue is int userId)
            {
                _currentOrders = _orderRepo.GetByUser(userId);
                OrderGrid.ItemsSource = _currentOrders;
            }
        }

        //private void AddOrder_Click(object sender, RoutedEventArgs e)
        //{
        //    if (UserComboBox.SelectedValue is int userId)
        //    {
        //        var order = new Order
        //        {
        //            UserId = userId,
        //            OrderDate = DateTime.Now,
        //            TotalAmount = 0m,
        //            PaymentStatus = "Pending",
        //            Notes = string.Empty
        //        };
        //        _orderRepo.CreateOrder(order, new List<OrderItem>());
        //        LoadOrders();
        //    }
        //}

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selectedOrder)
            {
                var detailWindow = new OrderDetailWindow(selectedOrder, _orderRepo);
                detailWindow.Owner = this;
                if (detailWindow.ShowDialog() == true)
                {
                    // Reload orders if needed
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Select an order to edit.");
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selected)
            {
                var result = MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancel", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    //remove in the database, however if delete an order,user should manually delete all items first, check constraint on this
                    if (selected.OrderItems.Count > 0)
                    {
                        MessageBox.Show("Please delete all order items before cancelling the order.", "Cannot Cancel", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    _orderRepo.DeleteOrder(selected.OrderId);
                    OrderGrid.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Select an order row to cancel.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = -1;
            MessageBox.Show("Returned to user selection. Select a tab to continue.");
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            //turnback to login windwo
            //should ask confirmation


            var loginWindow = new Login();
            loginWindow.Show();
            this.Close(); // Close the current window

        }
        private void OrdersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selectedOrder)
            {
                if (selectedOrder.OrderItems == null)
                {
                    MessageBox.Show("This order has no items or failed to load items.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var detailWindow = new OrderDetailWindow(selectedOrder, _orderRepo);
                detailWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No order selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
