using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoucherSales_BO;
using VoucherSales_Repositories;

namespace VoucherSales_WPF.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private readonly IUserRepository _userRepo;
        private User _currentUser;

        public ProfilePage()
        {
            InitializeComponent();
            _userRepo = new UserRepository();

            // Lấy user hiện tại từ App
            _currentUser = App.CurrentUser;

            // Bind lên controls
            txtUsername.Text = _currentUser.Username;
            txtFullName.Text = _currentUser.FullName;
            txtEmail.Text = _currentUser.Email;
            txtPhone.Text = _currentUser.Phone;
        }

        private void OnSaveProfile(object sender, RoutedEventArgs e)
        {
            // Đọc lại từ UI
            _currentUser.FullName = txtFullName.Text.Trim();
            _currentUser.Email = txtEmail.Text.Trim();
            _currentUser.Phone = txtPhone.Text.Trim();

            // Gọi repository để lưu
            var ok = _userRepo.UpdateProfile(_currentUser);
            if (!ok)
            {
                MessageBox.Show("Failed to save profile.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Cập nhật App.CurrentUser luôn
            App.CurrentUser = _currentUser;

            MessageBox.Show("Profile updated successfully.", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            var oldP = pbCurrent.Password;
            var newP = pbNew.Password;
            var conf = pbConfirm.Password;

            if (string.IsNullOrEmpty(oldP) || string.IsNullOrEmpty(newP))
            {
                MessageBox.Show("Please fill both fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (newP != conf)
            {
                MessageBox.Show("New passwords do not match.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool ok = _userRepo.ChangePassword(_currentUser.UserId, oldP, newP);
            if (!ok)
            {
                MessageBox.Show("Current password incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Password changed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            // clear boxes:
            pbCurrent.Clear();
            pbNew.Clear();
            pbConfirm.Clear();
        }
    }
}
