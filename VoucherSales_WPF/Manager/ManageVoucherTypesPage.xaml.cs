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
        private VoucherType? selectedVoucherType;
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

        private void dgVoucherTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVoucherTypes.SelectedItem is VoucherType vt)
            {
                selectedVoucherType = vt;
                txtVoucherTypeName.Text = vt.Name;
                txtDiscountType.Text = vt.DiscountType;
                txtDiscountValue.Text = vt.DiscountValue.ToString();
                txtMinOrderValue.Text = vt.MinOrderValue?.ToString() ?? "";
                txtTotalQuantity.Text = vt.TotalQuantity.ToString();
                dpValidFrom.SelectedDate = vt.ValidFrom;
                dpValidTo.SelectedDate = vt.ValidTo;
                txtCategory.Text = vt.Category;
                txtLocation.Text = vt.Location;
                txtDescription.Text = vt.Description ?? "";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var vt = new VoucherType
            {
                Name = txtVoucherTypeName.Text,
                DiscountType = txtDiscountType.Text,
                DiscountValue = decimal.TryParse(txtDiscountValue.Text, out var dv) ? dv : 0,
                MinOrderValue = decimal.TryParse(txtMinOrderValue.Text, out var mov) ? mov : null,
                TotalQuantity = int.TryParse(txtTotalQuantity.Text, out var tq) ? tq : 0,
                ValidFrom = dpValidFrom.SelectedDate ?? DateTime.Now,
                ValidTo = dpValidTo.SelectedDate ?? DateTime.Now,
                Category = txtCategory.Text,
                Location = txtLocation.Text,
                Description = txtDescription.Text,
                CreatedAt = DateTime.Now
            };

            bool success = VoucherTypeDAO.Instance.CreateVoucherType(vt);

            if (success)
            {
                MessageBox.Show("Voucher type added successfully.");
                LoadVoucherTypes();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Failed to add voucher type.");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVoucherType == null)
            {
                MessageBox.Show("Please select a voucher type to update.");
                return;
            }

            selectedVoucherType.Name = txtVoucherTypeName.Text;
            selectedVoucherType.DiscountType = txtDiscountType.Text;
            selectedVoucherType.DiscountValue = decimal.TryParse(txtDiscountValue.Text, out var dv) ? dv : 0;
            selectedVoucherType.MinOrderValue = decimal.TryParse(txtMinOrderValue.Text, out var mov) ? mov : null;
            selectedVoucherType.TotalQuantity = int.TryParse(txtTotalQuantity.Text, out var tq) ? tq : 0;
            selectedVoucherType.ValidFrom = dpValidFrom.SelectedDate ?? DateTime.Now;
            selectedVoucherType.ValidTo = dpValidTo.SelectedDate ?? DateTime.Now;
            selectedVoucherType.Category = txtCategory.Text;
            selectedVoucherType.Location = txtLocation.Text;
            selectedVoucherType.Description = txtDescription.Text;

            bool result = VoucherTypeDAO.Instance.UpdateVoucherType(selectedVoucherType);

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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVoucherType == null)
            {
                MessageBox.Show("Please select a voucher type to delete.");
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to delete '{selectedVoucherType.Name}'?", "Confirm Delete", MessageBoxButton.YesNo);

            if (confirm == MessageBoxResult.Yes)
            {
                bool success = VoucherTypeDAO.Instance.DeleteVoucherType(selectedVoucherType.VoucherTypeId);
                if (success)
                {
                    MessageBox.Show("Voucher type deleted successfully.");
                    LoadVoucherTypes();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to delete voucher type.");
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            dgVoucherTypes.SelectedIndex = -1;
        }

        private void ClearForm()
        {
            txtVoucherTypeName.Text = "";
            txtDiscountType.Text = "";
            txtDiscountValue.Text = "";
            txtMinOrderValue.Text = "";
            txtTotalQuantity.Text = "";
            dpValidFrom.SelectedDate = null;
            dpValidTo.SelectedDate = null;
            txtCategory.Text = "";
            txtLocation.Text = "";
            txtDescription.Text = "";
            selectedVoucherType = null;
        }
    }
}
