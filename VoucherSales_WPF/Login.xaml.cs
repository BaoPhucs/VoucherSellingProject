using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoucherSales_Repositories;

namespace VoucherSales_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private IUserRepository _userRepository = new UserRepository();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtLoginUsername.Text.Trim();
            string password = txtLoginPassword.Text;

            var user = _userRepository.ValidateLogin(username, password);

            if (user == null)
            {
                MessageBox.Show("Wrong username or password!", "Login failed",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            App.CurrentUser = user;

            Window nextWindow;

            switch (user.Role.RoleName)
            {
                case "Admin":
                    nextWindow = new AdminMainWindow();
                    break;

                case "Staff":
                    nextWindow = new StaffMainWindow();
                    break;

                case "Customer":
                    nextWindow = new CustomerMainWindow(user);
                    break;

                default:
                    MessageBox.Show($"Role “{user.Role.RoleName}” không hợp lệ.", "Lỗi phân quyền",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            nextWindow.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // 1. Đọc dữ liệu
            string username = txtRegisterUsername.Text.Trim();
            string fullname = txtRegisterFullName.Text.Trim();
            string email = txtRegisterEmail.Text.Trim();
            string pass = txtRegisterPassword.Text;
            string confirm = txtRegisterConfirmPassword.Text;

            // 2. Validate cơ bản
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(fullname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (pass != confirm)
            {
                MessageBox.Show("Password and Confirm Password do not match.", "Validation Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Gọi repository
            bool created = _userRepository.Register(fullname, username, email, pass);
            if (!created)
            {
                MessageBox.Show("Username or Email already exists.", "Register Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 4. Thành công
            MessageBox.Show("Registration successful! Please log in.", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

            // Clear form & chuyển về tab Login
            txtRegisterUsername.Clear();
            txtRegisterEmail.Clear();
            txtRegisterPassword.Clear();
            txtRegisterConfirmPassword.Clear();
            tbiLogin.IsSelected = true;
        }

    }
}