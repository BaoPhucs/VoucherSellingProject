﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF.Manager
{
    public partial class ManageUsersPage : UserControl
    {
        private readonly IUserRepository _repo = new UserRepository();

        private List<User> allUsers = new();
        private User? selectedUser;
        private List<Role> allRoles = new();

        public ManageUsersPage()
        {
            InitializeComponent();
            LoadRoles();
            LoadUsers();
        }

        private void LoadRoles()
        {
            allRoles = _repo.GetAllRoles();
            cmbRole.ItemsSource = allRoles;
        }

        private void LoadUsers()
        {
            allUsers = _repo.GetAllUsers();
            dgUsers.ItemsSource = allUsers;
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            cmbRole.SelectedIndex = -1;
            txtSearch.Text = "";
            selectedUser = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string kw = txtSearch.Text.Trim().ToLower();
            dgUsers.ItemsSource = string.IsNullOrEmpty(kw)
                ? allUsers
                : allUsers.Where(u =>
                    u.UserId.ToString().Contains(kw) ||
                    (u.Username?.ToLower().Contains(kw) ?? false) ||
                    (u.FullName?.ToLower().Contains(kw) ?? false))
                  .ToList();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) btnSearch_Click(sender, e);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            dgUsers.ItemsSource = allUsers;
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = dgUsers.SelectedItem as User;
            if (selectedUser == null) return;

            txtUsername.Text = selectedUser.Username;
            txtFullName.Text = selectedUser.FullName;
            txtEmail.Text = selectedUser.Email;
            txtPhone.Text = selectedUser.Phone;
            txtPassword.Password = selectedUser.PasswordHash;
            cmbRole.SelectedValue = selectedUser.RoleId;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRole.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò."); return;
            }

            bool ok = _repo.Register(
                fullname: txtFullName.Text.Trim(),
                username: txtUsername.Text.Trim(),
                email: txtEmail.Text.Trim(),
                phone: txtPhone.Text.Trim(),
                password: txtPassword.Password,
                roleId: (int)cmbRole.SelectedValue
            );

            MessageBox.Show(ok ? "Thêm người dùng thành công." : "Username hoặc Email đã tồn tại.");
            if (ok) { LoadUsers(); ClearForm(); }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Chọn người dùng để cập nhật."); return;
            }

            selectedUser.FullName = txtFullName.Text.Trim();
            selectedUser.Email = txtEmail.Text.Trim();
            selectedUser.Phone = txtPhone.Text.Trim();
            selectedUser.RoleId = (int)(cmbRole.SelectedValue ?? selectedUser.RoleId);

            bool ok = _repo.UpdateProfile(selectedUser);
            MessageBox.Show(ok ? "Cập nhật thành công." : "Cập nhật thất bại.");
            if (ok) LoadUsers();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Chọn người dùng để xoá."); return;
            }

            var confirm = MessageBox.Show(
                $"Xoá '{selectedUser.Username}' ?", "Xác nhận", MessageBoxButton.YesNo);

            if (confirm != MessageBoxResult.Yes) return;

            bool ok = _repo.Delete(selectedUser.UserId);
            MessageBox.Show(ok ? "Xoá thành công." : "Không thể xoá.");
            if (ok) { LoadUsers(); ClearForm(); }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            LoadUsers();
            dgUsers.SelectedIndex = -1;
        }
    }
}
