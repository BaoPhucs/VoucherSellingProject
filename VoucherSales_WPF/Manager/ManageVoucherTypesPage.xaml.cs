using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using VoucherSales_BO;
using VoucherSales_DAO;

namespace VoucherSales_WPF.Manager
{
    public partial class ManageVoucherTypesPage : UserControl
    {
        private List<VoucherType> allVoucherTypes = new List<VoucherType>();

        public ManageVoucherTypesPage()
        {
            InitializeComponent();
            LoadVoucherTypes();
        }

        private void LoadVoucherTypes()
        {
            allVoucherTypes = VoucherTypeDAO.Instance.GetAllVoucherTypes();
            dgVoucherTypes.ItemsSource = allVoucherTypes;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgVoucherTypes.SelectedItem is VoucherType vt)
            {
                bool result = VoucherTypeDAO.Instance.UpdateVoucherType(vt);
                if (result)
                {
                    MessageBox.Show("Voucher type updated successfully.");
                    LoadVoucherTypes();
                }
                else
                {
                    MessageBox.Show("Failed to update voucher type.");
                }
            }
            else
            {
                MessageBox.Show("Please select a voucher type to update.");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgVoucherTypes.SelectedItem is VoucherType vt)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete '{vt.Name}'?", "Confirm Delete", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    bool success = VoucherTypeDAO.Instance.DeleteVoucherType(vt.VoucherTypeId);
                    if (success)
                    {
                        MessageBox.Show("Voucher type deleted successfully.");
                        LoadVoucherTypes();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete voucher type.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a voucher type to delete.");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadVoucherTypes();
            dgVoucherTypes.SelectedIndex = -1;
        }

        private void dgVoucherTypes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var vt = e.Row.Item as VoucherType;
                if (vt != null && vt.VoucherTypeId == 0) // New item, assuming ID is auto-generated
                {
                    vt.CreatedAt = DateTime.Now;
                    bool success = VoucherTypeDAO.Instance.CreateVoucherType(vt);
                    if (success)
                    {
                        MessageBox.Show("Voucher type added successfully.");
                        LoadVoucherTypes();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add voucher type.");
                    }
                }
            }
        }
    }
}
