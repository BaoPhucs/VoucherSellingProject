using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using VoucherSales_BO;
using VoucherSales_DAO;
using VoucherSales_Repositories;

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


        public StaffMainWindow()
        {
            InitializeComponent();
            LoadUsers();
            LoadMetaData();
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

        private void LoadVouchers()
        {
            if (UserComboBox.SelectedValue is int userId)
            {
                _currentVouchers = _voucherRepo.GetMyWalletVouchers(userId);
                   // load *all* vouchers, not just unredeemed

                VoucherGrid.ItemsSource = _currentVouchers;
            }
        }

        private void AddVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedValue is int userId)
            {
                var voucher = new Voucher
                {
                    VoucherId = Guid.NewGuid(),
                    VoucherTypeId = 0, // set appropriately
                    Code = "newCode",
                    IsRedeemed = false,
                    IssuedToUserId = userId,
                    CreatedAt = DateTime.Now
                };
                _currentVouchers.Add(voucher);
                VoucherGrid.Items.Refresh();
            }
        }

        private void EditVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (VoucherGrid.SelectedItem is Voucher selected)
            {
                selected.IsRedeemed = !selected.IsRedeemed;
                _voucherRepo.Redeem(selected.VoucherId, null);
                LoadVouchers();
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
                _currentVouchers.Remove(selected);
                LoadVouchers();            }
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

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedValue is int userId)
            {
                var order = new Order
                {
                    OrderId = new Random().Next(10000, 99999),
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0m,
                    PaymentStatus = "Pending",
                    Notes = string.Empty
                };
                _orderRepo.CreateOrder(order, new List<OrderItem>());
                LoadOrders();
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selected)
            {
                var result = MessageBox.Show("Do you want to save changes to this order?", "Confirm Save", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var allowed = (List<string>)Resources["PaymentStatusList"];
                    if (!allowed.Contains(selected.PaymentStatus))
                    {
                        MessageBox.Show($"Invalid status '{selected.PaymentStatus}'. Allowed: {string.Join(", ", allowed)}");
                        return;
                    }
                    try
                    {
                        _orderRepo.UpdateOrderStatus(selected.OrderId, selected.PaymentStatus);
                        LoadOrders();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error saving order: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Select an order row to edit, then click Save.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selected)
            {
                var result = MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancel", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _currentOrders.Remove(selected);
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
    }
}
