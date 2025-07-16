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
                if (!ValidateVoucherType(vt, out string error))
                {
                    MessageBox.Show(error);
                    return;
                }

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






        private void dgVoucherTypes_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new VoucherType
            {
                CreatedAt = DateTime.Now,
                Name = "New Voucher",          // Optional default
                Description = "",              // Optional default
                                               // Optional default
            };
        }

        private bool ValidateVoucherType(VoucherType vt, out string error)
        {
            if (string.IsNullOrWhiteSpace(vt.Name))
            {
                error = "Name is required.";
                return false;
            }
            if (vt.DiscountType != "FixedAmount" && vt.DiscountType != "Percentage")
            {
                error = "DiscountType must be 'FixedAmount' or 'Percentage'.";
                return false;
            }
            if (vt.DiscountValue < 0)
            {
                error = "DiscountValue must be >= 0.";
                return false;
            }
            if (vt.MinOrderValue.HasValue && vt.MinOrderValue < 0)
            {
                error = "MinOrderValue must be >= 0.";
                return false;
            }
            if (vt.TotalQuantity < 0)
            {
                error = "TotalQuantity must be >= 0.";
                return false;
            }
            if (vt.ValidFrom >= vt.ValidTo)
            {
                error = "ValidFrom must be before ValidTo.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        private void dgVoucherTypes_RowEditEnding_1(object sender, DataGridRowEditEndingEventArgs e)
        {
            dgVoucherTypes.RowEditEnding -= dgVoucherTypes_RowEditEnding_1; // Unsubscribe temporarily
            try
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    dgVoucherTypes.CommitEdit(DataGridEditingUnit.Row, true);

                    if (e.Row.Item is VoucherType vt)
                    {
                        if (!ValidateVoucherType(vt, out string error))
                        {
                            MessageBox.Show(error);
                            return;
                        }


                        if (vt.VoucherTypeId == 0)
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
                        else
                        {
                            VoucherTypeDAO.Instance.UpdateVoucherType(vt);
                        }
                    }
                }
            }
            finally
            {
                dgVoucherTypes.RowEditEnding += dgVoucherTypes_RowEditEnding_1; // Resubscribe
            }
        }

    }
}
