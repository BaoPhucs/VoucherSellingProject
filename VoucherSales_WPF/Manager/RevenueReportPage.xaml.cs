using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Drawing;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF.Manager
{
    public partial class RevenueReportPage : UserControl
    {
        private readonly IOrderRepository _orderRepo = new OrderRepository();

        public RevenueReportPage()
        {
            InitializeComponent();
            SetDefaultDates();
            LoadReport();
            _orderRepo = new OrderRepository();
        }

        private void SetDefaultDates()
        {
            dpFrom.SelectedDate = DateTime.Today.AddMonths(-1);
            dpTo.SelectedDate = DateTime.Today;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (dpFrom.SelectedDate == null || dpTo.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn khoảng ngày hợp lệ.");
                return;
            }

            DateTime from = dpFrom.SelectedDate.Value.Date;
            DateTime to = dpTo.SelectedDate.Value.Date.AddDays(1).AddTicks(-1);

            LoadReport(from, to);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultDates();
            LoadReport();
        }

        private void LoadReport(DateTime? from = null, DateTime? to = null)
        {
            DateTime start = from ?? DateTime.Today.AddMonths(-1);
            DateTime end = to ?? DateTime.Today;

            var orders = _orderRepo.GetOrdersByDateRange(start, end);

            dgOrders.ItemsSource = orders;

            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            int totalOrders = orders.Count;

            txtTotalRevenue.Text = $"Tổng doanh thu: {totalRevenue:C0}";
            txtTotalOrders.Text = $"Số đơn hàng: {totalOrders}";
        }
    }
}
