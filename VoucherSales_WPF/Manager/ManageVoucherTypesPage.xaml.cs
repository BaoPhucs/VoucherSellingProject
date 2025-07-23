using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF.Manager
{
    public partial class ManageVoucherTypesPage : UserControl
    {
        private readonly IVoucherTypeRepository _repo = new VoucherTypeRepository();

        private VoucherType? selectedVT;
        private List<VoucherType> allVoucherTypes = new();

        public ManageVoucherTypesPage()
        {
            InitializeComponent();
            LoadVoucherTypes();
            dpValidFrom.SelectedDate = DateTime.Now;
            dpValidTo.SelectedDate = DateTime.Now.AddMonths(6);
        }

        private void LoadVoucherTypes()
        {
            allVoucherTypes = _repo.GetAll();
            dgVoucherTypes.ItemsSource = allVoucherTypes;
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtDescription.Text = "";
            cbDiscountType.SelectedIndex = -1;
            txtDiscountValue.Text = "";
            txtMinOrder.Text = "";
            txtTotalQty.Text = "";
            txtSearch.Text = "";
            dpValidFrom.SelectedDate = DateTime.Now;
            dpValidTo.SelectedDate = DateTime.Now.AddMonths(6);
            selectedVT = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string kw = txtSearch.Text.Trim().ToLower();
            dgVoucherTypes.ItemsSource = string.IsNullOrEmpty(kw)
                ? allVoucherTypes
                : allVoucherTypes.Where(v =>
                    v.VoucherTypeId.ToString().Contains(kw) ||
                    (v.Name?.ToLower().Contains(kw) ?? false))
                .ToList();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) btnSearch_Click(sender, e);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            dgVoucherTypes.ItemsSource = allVoucherTypes;
        }

        private void dgVoucherTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedVT = dgVoucherTypes.SelectedItem as VoucherType;
            if (selectedVT == null) return;

            txtName.Text = selectedVT.Name;
            txtDescription.Text = selectedVT.Description;
            cbDiscountType.Text = selectedVT.DiscountType;
            txtDiscountValue.Text = selectedVT.DiscountValue.ToString();
            txtMinOrder.Text = selectedVT.MinOrderValue?.ToString() ?? "";
            txtTotalQty.Text = selectedVT.TotalQuantity.ToString();
            dpValidFrom.SelectedDate = selectedVT.ValidFrom;
            dpValidTo.SelectedDate = selectedVT.ValidTo;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtDiscountValue.Text, out var discount) ||
                !int.TryParse(txtTotalQty.Text, out var qty))
            {
                MessageBox.Show("Giá trị giảm hoặc Tổng SL không hợp lệ."); return;
            }

            var vt = new VoucherType
            {
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                DiscountType = (cbDiscountType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Percentage",
                DiscountValue = discount,
                MinOrderValue = string.IsNullOrWhiteSpace(txtMinOrder.Text) ? null : decimal.Parse(txtMinOrder.Text),
                TotalQuantity = qty,
                ValidFrom = dpValidFrom.SelectedDate ?? DateTime.Now,
                ValidTo = dpValidTo.SelectedDate ?? DateTime.Now.AddMonths(6),
                CreatedAt = DateTime.Now,
                Category = "General",
                Location = "All"
            };

            if (_repo.Create(vt))
            {
                MessageBox.Show("Thêm loại voucher thành công.");
                LoadVoucherTypes();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Không thể thêm. Có thể tên đã tồn tại.");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVT == null)
            {
                MessageBox.Show("Chọn dòng để cập nhật."); return;
            }

            if (!decimal.TryParse(txtDiscountValue.Text, out var discount) ||
                !int.TryParse(txtTotalQty.Text, out var qty))
            {
                MessageBox.Show("Giá trị giảm hoặc Tổng SL không hợp lệ."); return;
            }

            selectedVT.Name = txtName.Text.Trim();
            selectedVT.Description = txtDescription.Text.Trim();
            selectedVT.DiscountType = (cbDiscountType.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Percentage";
            selectedVT.DiscountValue = discount;
            selectedVT.MinOrderValue = string.IsNullOrWhiteSpace(txtMinOrder.Text) ? null : decimal.Parse(txtMinOrder.Text);
            selectedVT.TotalQuantity = qty;
            selectedVT.ValidFrom = dpValidFrom.SelectedDate ?? DateTime.Now;
            selectedVT.ValidTo = dpValidTo.SelectedDate ?? DateTime.Now.AddMonths(6);

            if (_repo.Update(selectedVT))
            {
                MessageBox.Show("Cập nhật thành công.");
                LoadVoucherTypes();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại.");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVT == null)
            {
                MessageBox.Show("Chọn dòng để xoá."); return;
            }

            var ok = MessageBox.Show($"Xoá VoucherType '{selectedVT.Name}'?", "Xác nhận", MessageBoxButton.YesNo);
            if (ok != MessageBoxResult.Yes) return;

            if (_repo.Delete(selectedVT.VoucherTypeId))
            {
                MessageBox.Show("Xoá thành công.");
                LoadVoucherTypes();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Không thể xoá (đang được dùng?).");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            LoadVoucherTypes();
            dgVoucherTypes.SelectedIndex = -1;
        }
    }
}
